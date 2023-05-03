using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AmazonInternal.ThirdParty;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
public class AmazonSDKManager : EditorWindow {
    static string AmazonSdkVersion;
    static AddRequest Request;

    static void AddEditorCoroutine () {
        // Add a package to the Project
        Request = Client.Add ("com.unity.editorcoroutine");
        EditorApplication.update += Progress;
    }

    static void Progress () {
        if (Request.IsCompleted) {
            if (Request.Status == StatusCode.Success)
                Debug.Log ("Installed: " + Request.Result.packageId);
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log (Request.Error.message);

            EditorApplication.update -= Progress;
        }
    }

    [MenuItem ("Amazon/About Amazon SDK", false, 0)]
    public static void About () {
        AmazonAboutDialog.ShowDialog ();
    }

    [MenuItem ("Amazon/Documentation...", false, 1)]
    public static void Documentation () {
        Application.OpenURL (AmazonConstants.docUrl);
    }

    [MenuItem ("Amazon/Manage SDKs...", false, 4)]
    public static void SdkManagerProd () {
        AmazonSDKManager.ShowSDKManager ();
    }

    private const string downloadDir = "Assets/Amazon";

    private struct SdkInfo {
        public string Name;
        public string Key;
        public string Url;
        public string LatestVersion;
        public string CurrentVersion;
        public string Filename;

        public bool FromJson (string name, Dictionary<string, object> dict) {
            if (string.IsNullOrEmpty (name) || dict == null)
                return false;

            object obj;
            Key = name;
            if (dict.TryGetValue ("name", out obj))
                Name = obj as string;
            if (dict.TryGetValue ("link", out obj))
                Url = obj as string;
            if (dict.TryGetValue ("version", out obj))
                LatestVersion = obj as string;
            if (!string.IsNullOrEmpty (Url)) {
                var uri = new Uri (Url);
                var path = uri.IsAbsoluteUri ? uri.AbsolutePath : uri.LocalPath;
                Filename = Path.GetFileName (path);
            }
            return true;
        }

        public bool FromConfig(AmazonPackageConfig config)
        {
            if (config == null || string.IsNullOrEmpty(config.Name) || !string.IsNullOrEmpty(Key) && Key != config.Name)
                return false;
            CurrentVersion = config.Version;
            return true;
        }
    }

    // Version and download info for the SDK and network mediation adapters.
    private static SdkInfo amazonSdkInfo;
    private static readonly SortedDictionary<string, SdkInfo> sdkInfo = new SortedDictionary<string, SdkInfo> ();

    // Async download operations tracked here.
    private AmazonCoroutines.AmazonCoroutine coroutine;
    private UnityWebRequest downloader;
    private string activity;

    // Custom style for LabelFields.  (Don't make static or the dialog doesn't recover well from Unity compiling
    // a new or changed editor script.)
    private GUIStyle labelStyle;
    private GUIStyle labelStyleArea;
    private GUIStyle labelStyleLink;
    private GUIStyle headerStyle;
    private readonly GUILayoutOption fieldWidth = GUILayout.Width (60);

    private Vector2 scrollPos;

    public static void ShowSDKManager () {
        var win = GetWindow<AmazonSDKManager> (true);
        win.titleContent = new GUIContent ("Amazon SDK Manager");
        win.Focus ();
    }

    void Awake () {
        labelStyle = new GUIStyle (EditorStyles.label) {
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };
        labelStyleArea = new GUIStyle (EditorStyles.label) {
            wordWrap = true
        };
        labelStyleLink = new GUIStyle (EditorStyles.label) {
            normal = { textColor = Color.blue },
            active = { textColor = Color.white },
        };
        headerStyle = new GUIStyle (EditorStyles.label) {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            fixedHeight = 18
        };
        CancelOperation ();
    }

    void OnEnable () {
        coroutine = this.StartCoroutine (GetSDKVersions ());
    }

    void OnDisable () {
        CancelOperation ();
    }

    public void deleteUnwantedFiles()
    {
        String[] filesToDel =
            {
                "Assets/Amazon/Scripts/Internal/AdResponce.cs",
                "Assets/Amazon/Scripts/Internal/AdResponce.cs.meta",
                "Assets/Amazon/Scripts/Internal/IOS/IOSAdResponce.cs",
                "Assets/Amazon/Scripts/Internal/IOS/IOSAdResponce.cs.meta",
                "Assets/Amazon/Scripts/Internal/Android/AndroidAdResponce.cs",
                "Assets/Amazon/Scripts/Internal/Android/AndroidAdResponce.cs.meta",
                "Assets/Amazon/Plugins/Android/aps-sdk.aar",
                "Assets/Amazon/Plugins/Android/aps-sdk.aar.meta",
                "Assets/Amazon/Scripts/Mediations/AdMobMediation/Plugins/Android/aps-admob-adapter.aar",
                "Assets/Amazon/Scripts/Mediations/AdMobMediation/Plugins/Android/aps-admob-adapter.aar.meta",
                "Assets/Amazon/Scripts/Mediations/APSConnectionUtil",
                "Assets/Amazon/Scripts/Mediations/APSConnectionUtil.meta",
                "Assets/Amazon/Scripts/Mediations/MoPubMediation",
                "Assets/Amazon/Scripts/Mediations/MoPubMediation.meta",
                "Assets/APSConnectionUtil",
                "Assets/APSConnectionUtil.meta",
                "Assets/Amazon/Sample/AmazonMoPubDemo.cs",
                "Assets/Amazon/Sample/AmazonMoPubDemo.cs.meta",
                "Assets/Amazon/Sample/APSMoPubMediation.unity",
                "Assets/Amazon/Sample/APSMoPubMediation.unity.meta"
            };

        foreach (String fileToDel in filesToDel)
        {
            if (Directory.Exists(fileToDel))
            {
                Directory.Delete(fileToDel, true);
            } else if (File.Exists(fileToDel))
            {
                File.Delete(fileToDel);
            }
            
        }
    }

    private IEnumerator GetSDKVersions () {
        // Wait one frame so that we don't try to show the progress bar in the middle of OnGUI().
        yield return null;

        activity = "Downloading SDK version manifest...";

        UnityWebRequest www = new UnityWebRequest (AmazonConstants.manifestURL) {
            downloadHandler = new DownloadHandlerBuffer (),
            timeout = 10, // seconds
        };
        yield return www.SendWebRequest ();

        if (!string.IsNullOrEmpty (www.error)) {
            Debug.LogError (www.error);
            EditorUtility.DisplayDialog (
                "SDK Manager Service",
                "The services we need are not accessible. Please consider integrating manually.\n\n" +
                "For instructions, see " + AmazonConstants.helpLink,
                "OK");
        }

        var json = www.downloadHandler.text;
        if (string.IsNullOrEmpty (json)) {
            json = "{}";
            Debug.LogError ("Unable to retrieve SDK version manifest.  Showing installed SDKs only.");
        }
        www.Dispose ();

        // Got the file.  Now extract info on latest SDKs available.
        amazonSdkInfo = new SdkInfo ();
        sdkInfo.Clear ();
        var dict = Json.Deserialize (json) as Dictionary<string, object>;
        if (dict != null) {
            object obj;
            if (dict.TryGetValue ("sdk", out obj)) {
                amazonSdkInfo.FromJson ("sdk", obj as Dictionary<string, object>);
                amazonSdkInfo.CurrentVersion = AmazonConstants.VERSION;
            }
            if (dict.TryGetValue ("adapters", out obj)){
                var info = new SdkInfo ();
                foreach (var item in obj as Dictionary<string, object>) {
                    if (info.FromJson (item.Key, item.Value as Dictionary<string, object>))
                            sdkInfo[info.Key] = info;
                }

            }
        }

        var baseType = typeof(AmazonPackageConfig);
        var configs = from t in Assembly.GetExecutingAssembly().GetTypes()
                      where t.IsSubclassOf(baseType) && !t.IsAbstract
                      select Activator.CreateInstance(t) as AmazonPackageConfig;
        foreach (var config in configs) {
            SdkInfo info;
            sdkInfo.TryGetValue(config.Name, out info);
            if (info.FromConfig(config) && info.Key != null)
                sdkInfo[info.Key] = info;
        }
        coroutine = null;

        deleteUnwantedFiles();
        Repaint ();
    }

    void OnGUI () {
        // Is any async job in progress?
        var stillWorking = coroutine != null || downloader != null;

        GUILayout.Space (5);
        EditorGUILayout.LabelField ("Amazon SDKs", labelStyle, GUILayout.Height (20));
        using (new EditorGUILayout.VerticalScope("box")) {
            SdkHeaders();
            SdkRow(amazonSdkInfo);
        }

        if (sdkInfo.Count > 0) {
            GUILayout.Space (5);
            EditorGUILayout.LabelField ("Mediated Networks", labelStyle, GUILayout.Height (20));
            using (new EditorGUILayout.VerticalScope ("box"))
            using (var s = new EditorGUILayout.ScrollViewScope (scrollPos, false, false)) {
                scrollPos = s.scrollPosition;
                SdkHeaders ();
                foreach (var item in sdkInfo)
                    SdkRow (item.Value);
            }
        }

        // Indicate async operation in progress.
        using (new EditorGUILayout.HorizontalScope (GUILayout.ExpandWidth (false)))
        EditorGUILayout.LabelField (stillWorking ? activity : " ");

        using (new EditorGUILayout.HorizontalScope ()) {
            GUILayout.Space (10);
            if (!stillWorking) {
                if (GUILayout.Button ("Done", fieldWidth))
                    Close ();
            } else {
                if (GUILayout.Button ("Cancel", fieldWidth))
                    CancelOperation ();
            }
            if (GUILayout.Button ("Help", fieldWidth))
                Application.OpenURL (AmazonConstants.helpLink);
        }
    }

    private void SdkHeaders () {
        using (new EditorGUILayout.HorizontalScope (GUILayout.ExpandWidth (false))) {
            GUILayout.Space (10);
            EditorGUILayout.LabelField ("Package", headerStyle);
            GUILayout.Button ("Version", headerStyle);
            GUILayout.Space (3);
            GUILayout.Button ("Action", headerStyle, fieldWidth);
            GUILayout.Button (" ", headerStyle, GUILayout.Width (1));
            GUILayout.Space (5);
        }
        GUILayout.Space (4);
    }

    private void SdkRow (SdkInfo info, Func<bool, bool> customButton = null) {
        var lat = info.LatestVersion;
        var cur = info.CurrentVersion;
        var isInst = !string.IsNullOrEmpty (cur);
        var canInst = !string.IsNullOrEmpty (lat) && (!isInst || AmazonUtils.CompareVersions (cur, lat) < 0);
        // Is any async job in progress?
        var stillWorking = coroutine != null || downloader != null;

        string tooltip = string.Empty;
        if (isInst && (AmazonUtils.CompareVersions (cur, lat) != 0))
            tooltip += "\n  Installed:  " + cur;
        if (!string.IsNullOrEmpty (tooltip))
            tooltip = info.Name + "\n  Package:  " + (lat ?? "n/a") + tooltip;

        GUILayout.Space (4);
        using (new EditorGUILayout.HorizontalScope (GUILayout.ExpandWidth (false))) {
            GUILayout.Space (10);
            EditorGUILayout.LabelField (new GUIContent { text = info.Name, tooltip = tooltip });
            GUILayout.Button (new GUIContent {
                text = lat ?? "--",
                    tooltip = tooltip
            }, canInst ? EditorStyles.boldLabel : EditorStyles.label);
            GUILayout.Space (3);
            if (customButton == null || !customButton (canInst)) {
                GUI.enabled = !stillWorking && (canInst);
                if (GUILayout.Button (new GUIContent {
                        text = isInst ? "Upgrade" : "Install",
                            tooltip = tooltip
                    }, fieldWidth))
                    this.StartCoroutine (DownloadSDK (info));
                GUI.enabled = true;
            }
                // Need to fill space so that the Install/Upgrade buttons all line up nicely.
            GUILayout.Button (" ", EditorStyles.label, GUILayout.Width (17));
            GUILayout.Space (5);
        }
        GUILayout.Space (4);
    }

    private void CancelOperation () {
        // Stop any async action taking place.

        if (downloader != null) {
            downloader.Abort (); // The coroutine should resume and clean up.
            return;
        }

        if (coroutine != null)
            this.StopCoroutine (coroutine.routine);
        coroutine = null;
        downloader = null;
    }

    private IEnumerator DownloadSDK (SdkInfo info) {
        var path = Path.Combine (downloadDir, info.Filename);

        activity = string.Format ("Downloading {0}...", info.Filename);
        Debug.Log (activity);

        // Start the async download job.
        downloader = new UnityWebRequest (info.Url) {
            downloadHandler = new DownloadHandlerFile (path),
            timeout = 60, // seconds
        };
        downloader.SendWebRequest ();

        // Pause until download done/cancelled/fails, keeping progress bar up to date.
        while (!downloader.isDone) {
            yield return null;
            var progress = Mathf.FloorToInt (downloader.downloadProgress * 100);
            if (EditorUtility.DisplayCancelableProgressBar ("Amazon SDK Manager", activity, progress))
                downloader.Abort ();
        }
        EditorUtility.ClearProgressBar ();

        if (string.IsNullOrEmpty (downloader.error))
            AssetDatabase.ImportPackage (path, true); // OK, got the file, so let the user import it if they want.
        else {
            var error = downloader.error;
            if (downloader.isNetworkError) {
                if (error.EndsWith ("destination host"))
                    error += ": " + info.Url;
            } else if (downloader.isHttpError) {
                switch (downloader.responseCode) {
                    case 404:
                        var file = Path.GetFileName (new Uri (info.Url).LocalPath);
                        error = string.Format ("File {0} not found on server.", file);
                        break;
                    default:
                        error = downloader.responseCode + "\n" + error;
                        break;
                }
            }

            Debug.LogError (error);
        }

        // Reset async state so the GUI is operational again.
        downloader.Dispose ();
        downloader = null;
        coroutine = null;
    }
}