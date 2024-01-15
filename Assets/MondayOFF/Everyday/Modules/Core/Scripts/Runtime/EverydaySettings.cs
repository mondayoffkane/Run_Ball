using UnityEngine;

namespace MondayOFF
{
    public class EverydaySettings : ScriptableObject
    {
        public static EverydaySettings Instance
        {
            get
            {
                if (_instance is null)
                {
#if UNITY_EDITOR
                    var settingAssets = UnityEditor.AssetDatabase.FindAssets("t:EverydaySettings");

                    if (settingAssets.Length != 1)
                    {
                        throw new UnityEditor.Build.BuildFailedException("[EVERYDAY] There should be ONLY 1 settings object.");
                    }

                    _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<EverydaySettings>(UnityEditor.AssetDatabase.GUIDToAssetPath(settingAssets[0]));
#else
                var assets = Resources.LoadAll<EverydaySettings>("EverydaySettings");
                if (assets == null || assets.Length <= 0) {
                    Debug.Log("NOT found, search all");
                    assets = Resources.LoadAll<EverydaySettings>("");
                }
                if (assets.Length != 1) {
                    EverydayLogger.Error($"Found 0 or multiple {typeof(EverydaySettings).Name}s in Resources folder. There should only be one.");
                } else {
                    _instance = assets[0];
                }
#endif
                    Debug.Assert(_instance != null, "EverydaySettings asset not found.");
                }

                return _instance;
            }
        }

        internal static AdSettings AdSettings => Instance?.adSettings;
        private static EverydaySettings _instance;

        [SerializeField] internal bool initializeOnLaunch = true;
        [SerializeField] internal float initializationDelay = 2f;
        [SerializeField] internal LogLevel logLevel = LogLevel.Warning;
        [SerializeField] internal bool isTestMode = false;
        [SerializeField] internal AdSettings adSettings = default;
        [SerializeField] internal string gameId = "";
        [Header("Splash Screen")]
        [PreviewSprite]
        [SerializeField] internal Sprite companyLogo = default;
        [DisableEditing]
        [SerializeField] internal string gameSceneName = default;

        public static LogLevel GetLogLevel()
        {
            return Instance.logLevel;
        }
        public static void SetLogLevel(LogLevel logLevel)
        {
            Instance.logLevel = logLevel;
        }
    }
}