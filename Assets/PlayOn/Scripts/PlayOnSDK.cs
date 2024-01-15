using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
#if UNITY_IOS
using AOT;
#endif
public class PlayOnSDK
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public const string BundleClassName = "android.os.Bundle";
    public const string DateClassName = "java.util.Date";
    public const string HashClassName = "java.util.HashMap";

    private static AndroidJavaObject _androidBridge; 

    private static AndroidJavaObject getBridge ()
	{
		if (_androidBridge == null)
			using (var pluginClass = new AndroidJavaClass( AndroidBridge ))
				_androidBridge = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
		return _androidBridge;
	}

	private readonly static string AndroidBridge = "com.playon.bridge.PlayOnManager";
#endif

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void _playOnInitialize(string apiKey, string iosStoreId);
    [DllImport("__Internal")]
    public static extern void _playOnSetEngineInfo(string engineName, string engineVersion);
    [DllImport("__Internal")]
    public static extern bool _playOnIsInitialized();
    [DllImport("__Internal")]
    public static extern string _playOnGetConsentString();
    [DllImport("__Internal")]
    public static extern bool _playOnIsGeneralConsentGiven();
    [DllImport("__Internal")]
    public static extern void _playOnClearConsentString();
    [DllImport("__Internal")]
    public static extern void _playOnSetConsentString(string consentString);
    [DllImport("__Internal")]
    public static extern void _playOnSetGdprConsent(bool consent);
    [DllImport("__Internal")]
    public static extern void _playOnSetGdprConsentWithString(bool consent, string consentString);
    [DllImport("__Internal")]
    public static extern void _playOnSetDoNotSell(bool isApplied);
    [DllImport("__Internal")]
    public static extern void _playOnSetDoNotSellWithString(bool isApplied, string consentString);
    [DllImport("__Internal")]
    public static extern void _playOnForceRegulationType(int type);
    [DllImport("__Internal")]
    public static extern void _playOnClearForceRegulationType();
    [DllImport("__Internal")]
    public static extern int _playOnGetRegulationType();
    [DllImport("__Internal")]
    public static extern float _playOnGetDeviceVolumeLevel();
    [DllImport("__Internal")]
    public static extern void _playOnSetIsChildDirected(bool flag);
    [DllImport("__Internal")]
    public static extern void _playOnSetLogLevel(int level);
    [DllImport("__Internal")]
    public static extern void _playOnRequestTrackingAuthorization();
    [DllImport("__Internal")]
    public static extern void _playOnAddCustomAttribute(string key, string value);
    [DllImport("__Internal")]
    public static extern void _playOnClearCustomAttributes();
    [DllImport("__Internal")]
    public static extern void _playOnRemoveCustomAttribute(string key);
    [DllImport("__Internal")]
    public static extern void _playOnSetPlayerID(string id);
    [DllImport("__Internal")]
    public static extern string _playOnGetPlayerID();
    [DllImport("__Internal")]
    public static extern IntPtr _playOnSetOnInitializationListener(IntPtr callbackRef, PlayOnListener.PlayOnNoArgsDelegateNative onInitializationFinished, PlayOnListener.PlayOnNoArgsDelegateNative onInitializationFailed);
    [DllImport("__Internal")]
    public static extern List<SortedDictionary<String, String>> _playOnGetCustomAttributes();
    [DllImport("__Internal")]
    public static extern List<SortedDictionary<String, String>> _playOnGetCustomAttributes(string key);
    [DllImport("__Internal")]
    public static extern float _playOnGetDeviceScale();
    [DllImport("__Internal")]
    public static extern void _playOnPause();
    [DllImport("__Internal")]
    public static extern void _playOnResume();
#endif

#if UNITY_EDITOR
    private static int _editorDpi = 0;
    private static bool settedDPI = false;
    private static LogLevel editorloglevel = LogLevel.Debug;
#endif

    public static string SDK_VERSION = "2.2.5";

    public enum LogLevel
    {
        None,
        Info,
        Debug
    }

    public enum Position
    {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        Centered,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public enum AdUnitType
    {
        AudioBannerAd,
        AudioRewardedBannerAd,
        AudioLogoAd,
        AudioRewardedLogoAd
    }

    public enum AdUnitRewardType
    {
        InLevel,
        EndLevel
    }

    public enum AdUnitActionButtonType
    {
        Mute,
        Close,
        None
    }

    public enum ConsentType
    {
        Undefined,
        None,
        Gdpr,
        Ccpa
    }

    public enum AdSizingMethod{
        Flexible,
        Strict
    }

    public delegate void PlayOnNoArgsDelegate();
    public delegate void PlayOnStateDelegate(bool flag);
    public delegate void PlayOnImpressionDelegate(AdUnit.ImpressionData data);
    public delegate void PlayOnFloatDelegate(float amount);
    public delegate void PlayOnErrorDelegate(int errorParam, String error);

    public class PlayOnListener
#if UNITY_ANDROID && !UNITY_EDITOR
    : AndroidJavaProxy
#endif
    {
        public PlayOnSDK.PlayOnNoArgsDelegate OnInitializationFinished = () => { };
        public PlayOnSDK.PlayOnErrorDelegate OnInitializationFailed = (errorParam, error) => { };

#if UNITY_ANDROID && !UNITY_EDITOR
        public PlayOnListener() : base ("com.playon.bridge.common.SdkInitializationListener") { }

        void onInitializationFinished () {
            UnityMainThreadDispatcher.Instance().Enqueue( () => OnInitializationFinished() ) ;
        }

        void onInitializationFailed (int errorParam, String error) {
            UnityMainThreadDispatcher.Instance().Enqueue( () => OnInitializationFailed(errorParam, error) ) ;
        }
#endif

#if UNITY_IOS && !UNITY_EDITOR
        public IntPtr playOnNativeListenerRef;

        public delegate void PlayOnNoArgsDelegateNative (IntPtr client);

        private static PlayOnListener IntPtrToClient(IntPtr cl){
            GCHandle handle = (GCHandle)cl;
            return handle.Target as PlayOnListener;
        }

        [MonoPInvokeCallback(typeof(PlayOnNoArgsDelegateNative ))]
        public static void OnInitializationFinishedNative(IntPtr client){
            PlayOnListener listener = IntPtrToClient(client);
            UnityMainThreadDispatcher.Instance().Enqueue( () => listener.OnInitializationFinished() ) ;
        }

        [MonoPInvokeCallback(typeof(PlayOnNoArgsDelegateNative ))]
        public static void OnInitializationFailedNative(IntPtr client){
            PlayOnListener listener = IntPtrToClient(client);
            UnityMainThreadDispatcher.Instance().Enqueue( () => listener.OnInitializationFailed(0, "") ) ;
        }
#endif
    }

    private static PlayOnListener playOnListener = new PlayOnListener();

    public static PlayOnSDK.PlayOnNoArgsDelegate OnInitializationFinished
    {
        get
        {
            return playOnListener.OnInitializationFinished;
        }
        set
        {
            playOnListener.OnInitializationFinished = value;
        }
    }

    public static PlayOnSDK.PlayOnErrorDelegate OnInitializationFailed
    {
        get
        {
            return playOnListener.OnInitializationFailed;
        }
        set
        {
            playOnListener.OnInitializationFailed = value;
        }
    }

    public static void Initialize(string apiKey, string iosStoreId = "")
    {
        UnityMainThreadDispatcher.Instance();
        string unityVersion = Application.unityVersion;
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {   
            getBridge ().Call("setEngineInformation", "unity_" + unityVersion, SDK_VERSION);
            getBridge ().Call("setOnInitializationListener", playOnListener);
            getBridge ().Call("initialize", activity, apiKey);
        }));
#elif UNITY_IOS && !UNITY_EDITOR
        playOnListener.playOnNativeListenerRef = _playOnSetOnInitializationListener((IntPtr)GCHandle.Alloc(playOnListener), PlayOnListener.OnInitializationFinishedNative, PlayOnListener.OnInitializationFailedNative);
        _playOnSetEngineInfo("unity", SDK_VERSION);
        _playOnInitialize(apiKey, iosStoreId);
#else

        PlayOnSDK.LogI(LogLevel.Info, "Initialization");
        OnInitializationFinished();
#endif
    }

    public static bool IsInitialized()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return getBridge ().Call<bool>("isInitialized");
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnIsInitialized();
#else
        LogI(LogLevel.Info,"Dummy Initialization. Default value true");
        return true;
#endif
    }

    public static void SetIsChildDirected(bool flag)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setIsChildDirected", flag);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetIsChildDirected(flag);
#endif  
    }

    /// <summary>
    /// Returns current device volume in Percentages from 0 to 100
    /// </summary>
    public static float GetDeviceVolumeLevel()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return getBridge ().Call<float>("getDeviceVolumeLevel");
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnGetDeviceVolumeLevel();
#else
        PlayOnSDK.LogW(LogLevel.Debug,"Editor mode is not supported. Returned value always 100");
        return 100.0f;
#endif
    }

    public static string GetConsentString()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return getBridge ().Call<string>("getConsentString");
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnGetConsentString();
#endif
        return "EditorIsNotSupported";
    }

    public static bool IsGeneralConsentGiven()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return getBridge ().Call<bool>("isGeneralConsentGiven");
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnIsGeneralConsentGiven();
#endif
        return false;
    }

    public static void ClearConsentString()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("clearConsentString");
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnClearConsentString();
#endif
    }

    public static void SetConsentString(string consentString)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setConsentString", consentString);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetConsentString(consentString);
#endif
    }

    public static void SetGdprConsent(bool consent)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setGdprConsent", consent);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetGdprConsent(consent);
#endif
    }

    public static void SetGdprConsent(bool consent, string consentString)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setGdprConsent", consent, consentString);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetGdprConsentWithString(consent, consentString);
#endif
    }

    public static void SetDoNotSell(bool isApplied)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setDoNotSell", isApplied);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetDoNotSell(isApplied);
#endif
    }

    public static void SetDoNotSell(bool isApplied, string consentString)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setDoNotSell", isApplied, consentString);
#elif UNITY_IOS && !UNITY_EDITOR
       _playOnSetGdprConsentWithString(isApplied, consentString);
#endif
    }

    public static void ForceRegulationType(ConsentType type)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass consentEnum = new AndroidJavaClass ("com.playon.bridge.dto.consent.ConsentType");
        AndroidJavaObject curType = consentEnum.CallStatic<AndroidJavaObject> ("valueOf", type.ToString ());
        getBridge ().Call("forceRegulationType", curType);
#elif UNITY_IOS && !UNITY_EDITOR
       _playOnForceRegulationType((int)type);
#endif
    }

    public static void ClearForceRegulationType()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("clearForceRegulationType");
#elif UNITY_IOS && !UNITY_EDITOR
       _playOnClearForceRegulationType();
#endif
    }

    public static ConsentType GetRegulationType()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject consentEnum = getBridge().Call<AndroidJavaObject>("getRegulationType");
        int typeIndex = consentEnum.Call<int> ("ordinal");
        return (ConsentType)typeIndex;
#elif UNITY_IOS && !UNITY_EDITOR
        return (ConsentType)_playOnGetRegulationType();
#endif
        LogI(LogLevel.Info, "GetRegulationType returns default value Undefined");
        return ConsentType.Undefined;
    }

    public static void AddCustomAttribute(string key, string value)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("addCustomAttribute", key, value);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnAddCustomAttribute(key, value);
#endif
    }

    public static void ClearCustomAttributes()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("clearCustomAttributes");
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnClearCustomAttributes();
#endif
    }

    public static void RemoveCustomAttribute(string key)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("removeCustomAttribute", key);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnRemoveCustomAttribute(key);
#endif
    }

    public static List<SortedDictionary<String, String>> GetCustomAttributes()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return getBridge ().Call<List<SortedDictionary<String, String>>>("getCustomAttributes");
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnGetCustomAttributes();
#else
        return new List<SortedDictionary<String, String>>();
#endif
    }

    public static List<SortedDictionary<String, String>> GetCustomAttributes(string key)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return getBridge ().Call<List<SortedDictionary<String, String>>>("getCustomAttributes", key);
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnGetCustomAttributes(key);
#else
        return new List<SortedDictionary<String, String>>();
#endif
    }

    public static void SetLogLevel(LogLevel level)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass typeEnum = new AndroidJavaClass ("com.playon.bridge.common.Log$LogLevel");
        AndroidJavaObject curType = typeEnum.CallStatic<AndroidJavaObject> ("valueOf", level.ToString ());
        getBridge ().Call("setLogLevel", curType);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetLogLevel((int)level);
#elif UNITY_EDITOR
        editorloglevel = level;
#endif
    }
    
    public static void LogE(LogLevel type, string message)
    {
#if UNITY_EDITOR
        switch (editorloglevel)
        {
            case LogLevel.Debug:
            Debug.LogError("PlayOnSDK: " +message);
            break;
            
            case LogLevel.Info:
            if(type == LogLevel.Info)
                Debug.LogError("PlayOnSDK: " +message);
            break;
            
            case LogLevel.None:
            break;
        }
#endif
    }
    
    public static void LogW(LogLevel type, string message)
    {
#if UNITY_EDITOR
        switch (editorloglevel)
        {
            case LogLevel.Debug:
                Debug.LogWarning("PlayOnSDK: " +message);
                break;
            
            case LogLevel.Info:
                if(type == LogLevel.Info)
                    Debug.LogWarning("PlayOnSDK: " +message);
                break;
            
            case LogLevel.None:
                break;
        }
#endif
    }
    
    public static void LogI(LogLevel type, string message)
    {
#if UNITY_EDITOR
        switch (editorloglevel)
        {
            case LogLevel.Debug:
                Debug.Log("PlayOnSDK: " +message);
                break;
            
            case LogLevel.Info:
                if(type == LogLevel.Info)
                    Debug.Log("PlayOnSDK: " +message);
                break;
            
            case LogLevel.None:
                break;
        }
#endif
    }
    
	public static void RequestTrackingAuthorization()
	{
#if UNITY_IOS && !UNITY_EDITOR
            _playOnRequestTrackingAuthorization();
#else
            PlayOnSDK.LogI(LogLevel.Info, "RequestTrackingAuthorization() ignonred. Requesting tracking authorization is made only for iOS platform.");
#endif
    }

    public static void SetPlayerID(string id){
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call("setPlayerID", id);
#elif UNITY_IOS && !UNITY_EDITOR
       _playOnSetPlayerID(id);
#endif    
    }

    public static string GetPlayerID(){
        string id = "";
#if UNITY_ANDROID && !UNITY_EDITOR
        id = getBridge ().Call<string>("getPlayerID");
#elif UNITY_IOS && !UNITY_EDITOR
       id = _playOnGetPlayerID();
#endif  
        return id;
    }

    public delegate void OnApplicationPause(bool isPaused);
    public static OnApplicationPause onApplicationPause = (isPaused) =>
    {
        if (isPaused) onPause();
        else onResume();
    };

    public static int GetOptimalEditorDPI()
    {
        int result = 96;

        int shortSide = Screen.width < Screen.height ? Screen.width : Screen.height;
        if (shortSide >= 1440)
            result = 440;
        else if(shortSide >= 1080)
            result = 323;
        else if(shortSide >= 720)
            result = 252;
        else if(shortSide >= 480)
            result = 170;
        
        return result;
    }
    
    public static void SetOptimalDPI()
    {
#if UNITY_EDITOR
        SetUnityEditorDPI(GetOptimalEditorDPI());
#endif
    }
    
    public static void SetUnityEditorDPI(int dpi)
    {
#if UNITY_EDITOR
        _editorDpi = dpi;
        settedDPI = true;
#endif
    }

    public static float GetUnityEditorDPI()
    {
#if UNITY_EDITOR
        return _editorDpi;
#else
        return Screen.dpi;
#endif
    }

    public static bool DPISettedByUser() {
#if UNITY_EDITOR
        return settedDPI;
#else
        return false;
#endif
    }
    public static float GetDeviceScale()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return GetUnityEditorDPI() / 160f;
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnGetDeviceScale();
#else
        return Application.platform == RuntimePlatform.Android
            ? GetUnityEditorDPI() / 160f
            : Mathf.Round(GetUnityEditorDPI() / 160f);
#endif
    }

    private static void onPause()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call ("OnPause");
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnPause();
#endif
    }

    private static void onResume()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        getBridge ().Call ("OnResume");
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnResume();
#endif
    }
}