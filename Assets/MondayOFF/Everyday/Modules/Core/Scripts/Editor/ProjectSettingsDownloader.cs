#if false
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Reflection;
using System.Collections.Generic;
using AppLovinMax.Scripts.IntegrationManager.Editor;
using System.Collections;
using System.Linq;

namespace MondayOFF {
    internal static class ProjectSettingsDownloader {
        const string APP_SETTINGS_URL = "https://everyday.mondayoff.me/development/project";

        const string ADMOB_NETWORK = "ADMOB_NETWORK";
        const string GOOGLE_AD_MANAGER_NETWORK = "GOOGLE_AD_MANAGER_NETWORK";

        internal async static void FetchAppSettings(EverydaySettingsWindow window) {
            try {
                if (string.IsNullOrEmpty(EverydaySettings.Instance.appId)) {
                    throw new System.ArgumentException("App ID is empty.");
                }
                window.isWorking = true;
                window.message = "Getting app settings...";

                var query = $"?gameId={EverydaySettings.Instance.appId}";

                EverydayLogger.Info($"Getting app settings for {EverydaySettings.Instance.appId}");

                HttpClient httpClient = new HttpClient() {
                    BaseAddress = new System.Uri(APP_SETTINGS_URL),
                };

                var response = await httpClient.GetAsync(query, window.cancellationTokenSource.Token);
                if (!response.IsSuccessStatusCode) {
                    throw new System.Exception($"Failed to get app settings: {response.StatusCode}");
                }

                EverydayLogger.Info("App settings downloaded successfully.");

                var responseString = await response.Content.ReadAsStringAsync();
                var appSettings = JsonUtility.FromJson<RemoteProjectSettings>(responseString);

                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, appSettings.AndroidBundleID);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, appSettings.iOSBundleID);
                EverydayLogger.Info($"\nBundle ID set\nAndroid\n\t{appSettings.AndroidBundleID}\niOS\n\t{appSettings.iOSBundleID}");

                Facebook.Unity.Settings.FacebookSettings.AppIds = new List<string> { appSettings.FacebookAppID };
                Facebook.Unity.Settings.FacebookSettings.ClientTokens = new List<string> { appSettings.FacebookClientToken };
                EditorUtility.SetDirty(Facebook.Unity.Settings.FacebookSettings.Instance);
                EverydayLogger.Info($"\nFacebook Settings set\nApp Id\n\t{appSettings.FacebookAppID}\nClient Token\n\t{appSettings.FacebookClientToken}");

                AppLovinSettings.Instance.AdMobAndroidAppId = appSettings.AndroidAdMobAppID;
                AppLovinSettings.Instance.AdMobIosAppId = appSettings.iOSAdMobAppID;
                AppLovinSettings.Instance.SaveAsync();
                EverydayLogger.Info($"\nAdMob ID set\nAndroid\n\t{appSettings.AndroidAdMobAppID}\niOS\n\t{appSettings.iOSAdMobAppID}");

                CheckAdMob(appSettings);

                // if unity username is mondayoffgames@gmail.com or *@mondayoff.me, set project ID too
                var userName = UnityEditor.CloudProjectSettings.userName;
                if (!string.IsNullOrEmpty(appSettings.UnityProjectID) && (userName == "mondayoffgames@gmail.com" || userName.EndsWith("@mondayoff.me"))) {
                    var type = Type.GetType("UnityEditor.Connect.UnityConnect, UnityEditor, Version = 0.0.0.0, Culture = neutral, PublicKeyToken = null");
                    PropertyInfo instanceInfo = type.GetProperty("instance");
                    object settingsInstance = instanceInfo.GetValue(null, null);

                    MethodInfo projectId = type.GetMethod("GetProjectGUID");
                    var currentId = projectId.Invoke(settingsInstance, new System.Object[] { }) as string;
                    if (currentId == appSettings.UnityProjectID) {
                        return;
                    }

                    MethodInfo unbind = type.GetMethod("UnbindProject");
                    MethodInfo bind = type.GetMethod("BindProject");

                    EverydayLogger.Info("Unbinding current cloud project");
                    unbind.Invoke(settingsInstance, new System.Object[] { });

                    var id = appSettings.UnityProjectID;

                    bind.Invoke(settingsInstance, new System.Object[] { id, null, null });
                    EverydayLogger.Info($"Bound cloud project to id: {id}");
                }
            } catch (TaskCanceledException) {
                EverydayLogger.Warn("Get app settings cancelled");
            } catch (System.ArgumentException e) {
                EverydayLogger.Warn(e.ToString());
            } finally {
                await Task.Delay(200);
                window.isWorking = false;
                window.message = "";
                window.Repaint();
            }
        }

        private static void CheckAdMob(in RemoteProjectSettings appSettings) {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) {
                if (string.IsNullOrEmpty(appSettings.AndroidAdMobAppID)) {
                    AppLovinEditorCoroutine.StartCoroutine(RemoveAdMob());
                } else {
                    AppLovinEditorCoroutine.StartCoroutine(AddAdMob());
                }
            } else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS) {
                if (string.IsNullOrEmpty(appSettings.iOSAdMobAppID)) {
                    AppLovinEditorCoroutine.StartCoroutine(RemoveAdMob());
                } else {
                    AppLovinEditorCoroutine.StartCoroutine(AddAdMob());
                }
            }
        }

        private static IEnumerator AddAdMob() {
            PluginData pluginData = null;
            yield return AppLovinMax.Scripts.IntegrationManager.Editor.AppLovinIntegrationManager.Instance.LoadPluginData(
                (data) => {
                    pluginData = data;
                }
            );
            var networks = pluginData.MediatedNetworks.Where(n => n.Name == ADMOB_NETWORK || n.Name == GOOGLE_AD_MANAGER_NETWORK);
            bool added = false;
            foreach (var network in networks) {
                // name of the folder after Max/Mediation/
                var name = network.DependenciesFilePath.Split('/').ElementAt(2);
                if (AppLovinIntegrationManager.IsAdapterInstalled(name)) {
                    continue;
                }
                yield return AppLovinMax.Scripts.IntegrationManager.Editor.AppLovinIntegrationManager.Instance.DownloadPlugin(network, false);
                added = true;
            }
            if (added) {
                AssetDatabase.Refresh();
                EverydayLogger.Info("AdMob adapter added");
            }
        }

        private static IEnumerator RemoveAdMob() {
            PluginData pluginData = null;
            yield return AppLovinMax.Scripts.IntegrationManager.Editor.AppLovinIntegrationManager.Instance.LoadPluginData(
                (data) => {
                    pluginData = data;
                }
            );
            var networks = pluginData.MediatedNetworks.Where(n => n.Name == ADMOB_NETWORK || n.Name == GOOGLE_AD_MANAGER_NETWORK);
            var pluginRoot = AppLovinMax.Scripts.IntegrationManager.Editor.AppLovinIntegrationManager.MediationSpecificPluginParentDirectory;
            foreach (var network in networks) {
                foreach (var pluginFilePath in network.PluginFilePaths) {
                    FileUtil.DeleteFileOrDirectory(System.IO.Path.Combine(pluginRoot, pluginFilePath));
                }
            }
            AssetDatabase.Refresh();
            EverydayLogger.Info("AdMob adapter removed");
        }
    }
}
#endif