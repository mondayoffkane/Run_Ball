
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;

namespace MondayOFF
{
    public static class Privacy
    {
        internal static int IS_GDPR_APPLICABLE => GetGDPRApplicable();
        internal static string TCString => GetTCF2String();
        internal static string CCPA_STRING = "";
        internal static bool HAS_ATT_CONSENT = true;

        private static string GetTCF2String()
        {
#if UNITY_EDITOR
            return null;
#endif
            string tcfString = null;
#if UNITY_ANDROID 
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass preferenceManagerClass = new AndroidJavaClass("android.preference.PreferenceManager");
            AndroidJavaObject sharedPreferences = preferenceManagerClass.CallStatic<AndroidJavaObject>("getDefaultSharedPreferences", currentActivity);
            tcfString = sharedPreferences.Call<string>("getString", "IABTCF_TCString", "");
#elif UNITY_IOS 
            tcfString = PlayerPrefs.GetString("IABTCF_TCString", null);
#endif
            return tcfString;
        }

        private static int GetGDPRApplicable()
        {
#if UNITY_EDITOR
            return 0;
#endif
            int isGdprApplicable = 0;

#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass preferenceManagerClass = new AndroidJavaClass("android.preference.PreferenceManager");
            AndroidJavaObject sharedPreferences = preferenceManagerClass.CallStatic<AndroidJavaObject>("getDefaultSharedPreferences", currentActivity);
            isGdprApplicable = sharedPreferences.Call<int>("getInt", "IABTCF_gdprApplies", 0);
#elif UNITY_IOS
            isGdprApplicable = PlayerPrefs.GetInt("IABTCF_gdprApplies", 0);
#endif
            return isGdprApplicable;
        }


#if UNITY_IOS && !UNITY_EDITOR
        public static void OpenAppSettings() {
            _OpenAppSettings();
        }
        [DllImport("__Internal")]
        private static extern void _OpenAppSettings();
#else
        public static void OpenAppSettings()
        {
            // TODO: Implement for Android Sandbox if needed
        }
#endif
    }
}
