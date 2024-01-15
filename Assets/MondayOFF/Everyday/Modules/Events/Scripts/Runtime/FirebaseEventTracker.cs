#if FIREBASE_ENABLED && !UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

namespace MondayOFF
{
    public static class EventTracker
    {
        private static Firebase.FirebaseApp _app = null;
        private static bool _isInitialized = false;
        private static System.Action _onInitialized = default;

        public static void TryStage(int stageNum, string stageName = "Stage")
        {
            if (!_isInitialized)
            {
                _onInitialized += () => TryStage(stageNum, stageName);
                return;
            }

            FirebaseAnalytics.LogEvent("Try",
                new Parameter(stageName, $"{stageName} {stageNum:000}")
            );
        }

        public static void ClearStage(int stageNum, string stageName = "Stage")
        {
            // Send event regardless of initialization
            switch (stageNum)
            {
                case 10:
                case 20:
                case 30:
                    SingularSDK.Event($"Stage_{stageNum}");
                    break;
            }

            if (!_isInitialized)
            {
                _onInitialized += () => ClearStage(stageNum, stageName);
                return;
            }

            FirebaseAnalytics.LogEvent("Clear",
                new Parameter(stageName, $"{stageName} {stageNum:000}")
            );
        }

        // Stringify prameter values
        public static void LogCustomEvent(string eventName, Dictionary<string, string> parameters)
        {
            if (!_isInitialized)
            {
                _onInitialized += () => LogCustomEvent(eventName, parameters);
                return;
            }

            if (parameters == null)
            {
                FirebaseAnalytics.LogEvent(eventName);
            }
            else
            {
                var eventParams = new Parameter[parameters.Count];
                int i = 0;
                foreach (var item in parameters)
                {
                    eventParams[i++] = new Parameter(item.Key, item.Value);
                }
                FirebaseAnalytics.LogEvent(eventName, eventParams);
            }
        }

        private static void TrackAdRevenue(string adUnitID, MaxSdkBase.AdInfo adInfo)
        {
            if (!_isInitialized) { return; }

            double revenue = adInfo.Revenue;
            var impressionParameters = new[] {
                new Parameter("ad_platform", "AppLovin"),
                new Parameter("ad_source", adInfo.NetworkName),
                new Parameter("ad_unit_name", adInfo.AdUnitIdentifier),
                new Parameter("ad_format", adInfo.AdFormat),
                new Parameter("value", revenue),
                new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };
            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            _isInitialized = false;
            Initialize();
        }

        internal static void Initialize()
        {
            if (_isInitialized)
            {
                EverydayLogger.Warn("Firebase already initialized");
                return;
            }
            if (!EveryDay.isInitialized)
            {
                EveryDay.OnEverydayInitialized += Initialize;
                return;
            }

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    _app = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    OnFirebaseInitialized();

                }
                else
                {
                    EverydayLogger.Error(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is NOT safe to use here.
                }
            });
        }

        private static void OnFirebaseInitialized()
        {
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += TrackAdRevenue;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += TrackAdRevenue;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += TrackAdRevenue;

            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);

            _isInitialized = true;
            _onInitialized?.Invoke();
        }
    }
}
#endif