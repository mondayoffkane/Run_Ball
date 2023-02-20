using UnityEngine;
using System;
#if UNITY_EDITOR
using System.Collections;
using System.Dynamic;
using UnityEditor;
using Object = UnityEngine.Object;
#endif
using System.Runtime.InteropServices;
public class AdUnit
{
    public PlayOnSDK.AdUnitType type;
    public float rewardValue = 0;
    public PlayOnSDK.AdUnitRewardType rewardType = PlayOnSDK.AdUnitRewardType.EndLevel;

#if UNITY_ANDROID && !UNITY_EDITOR
    protected AndroidJavaObject client;
#endif

#if UNITY_EDITOR
    protected GameObject stubLogo;
    protected bool editorAdAvailable = false;
#endif
#if UNITY_IOS && !UNITY_EDITOR
    protected IntPtr client;

    [DllImport("__Internal")]
    public static extern IntPtr _playOnCreateAudioAdUnit(int adType);
    [DllImport("__Internal")]
    public static extern void _playOnShow(IntPtr client);
    [DllImport("__Internal")]
    public static extern void _playOnClose(IntPtr client);
    [DllImport("__Internal")]
    public static extern void _playOnSetLogo(IntPtr client, int position, int xOffset, int yOffset, int size);
    [DllImport("__Internal")]
    public static extern bool _playOnIsAdAvailable(IntPtr client);
    [DllImport("__Internal")]
    public static extern IntPtr _playOnSetListeners(IntPtr client, IntPtr callbackRef, AdListener.PlayOnNoArgsDelegateNative onShow, AdListener.PlayOnNoArgsDelegateNative onClose, AdListener.PlayOnNoArgsDelegateNative onClick, AdListener.PlayOnStateDelegateNative onAvailabilityChange, AdListener.PlayOnFloatDelegateNative onReward, AdListener.PlayOnDataDelegateNative onImpression);
    [DllImport("__Internal")]
    public static extern IntPtr _playOnCreateMutableArray();
    [DllImport("__Internal")]
    public static extern void _playOnAddToMutableArray(IntPtr dictionary, int item);
    [DllImport("__Internal")]
    public static extern void _playOnSetReward(IntPtr client, int rewardType, float value);
    [DllImport("__Internal")]
    public static extern void _playOnSetPopup(IntPtr client, int location, int xOffset, int yOffset);
    [DllImport("__Internal")]
    public static extern void _playOnSetBanner(IntPtr client, int location);
    [DllImport("__Internal")]
    public static extern void _playOnSetVisualization(IntPtr client, string tint, string backgound);
    [DllImport("__Internal")]
    public static extern void _playOnSetActionButton(IntPtr client, int actionType, float delayTime);
    [DllImport("__Internal")]
    public static extern void _playOnSetProgressBar(IntPtr client, string tint);
    [DllImport("__Internal")]
    public static extern void _playOnDestroyBridgeReference(IntPtr obj);
    [DllImport("__Internal")]
    public static extern void _playOnTrackRewardedOffer(IntPtr obj);
    [DllImport("__Internal")]
    public static extern void _playOnAddAdUnitToRootView(IntPtr obj);
    [DllImport("__Internal")]
    public static extern void _playOnRemoveAdUnitFromSuperView(IntPtr obj);
#endif

    public const int AD_SIZE_LIMIT_DP_MIN = 70;
    public const int AD_SIZE_LIMIT_DP_MAX = 120;

    private float fadeValue = 0.1f;
    private float sceneVolumeValue = 1f;
    private int _xOffset = 50;
    private int _yOffset = 50;
    private int _size = 70;
    private PlayOnSDK.Position _location = PlayOnSDK.Position.TopLeft;
    private bool adBlocked = false;

    private AdListener adListener = new AdListener();
    public AdListener AdCallbacks
    {
        get
        {
            return adListener;
        }
    }

    public class ImpressionData
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject client;

        public ImpressionData(AndroidJavaObject ptr){
            client = ptr;
        }
#elif UNITY_IOS && !UNITY_EDITOR
        IntPtr client;

        [DllImport("__Internal")]
        public static extern string _playOnImpressionGetPlacementID(IntPtr obj);
            [DllImport("__Internal")]
        public static extern string _playOnImpressionGetSessionID(IntPtr obj);
            [DllImport("__Internal")]
        public static extern int _playOnImpressionGetAdType(IntPtr obj);
            [DllImport("__Internal")]
        public static extern string _playOnImpressionGetCountry(IntPtr obj);
            [DllImport("__Internal")]
        public static extern double _playOnGetRevenue(IntPtr obj);

        public ImpressionData(IntPtr ptr){
            client = ptr;
        }
#endif
        
#if UNITY_EDITOR
        private PlayOnSDK.AdUnitType adUnitType;
        public ImpressionData(PlayOnSDK.AdUnitType type) {
            adUnitType = type;
        }
#endif
        public string GetPlacementID(){
#if UNITY_ANDROID && !UNITY_EDITOR
            return client.Call<string>("getPlacementID");
#elif UNITY_IOS && !UNITY_EDITOR
            return _playOnImpressionGetPlacementID(client);
#elif UNITY_EDITOR
            return Guid.Empty.ToString();
#else
            return null;
#endif
        }

        public string GetSessionID(){
#if UNITY_ANDROID && !UNITY_EDITOR
            return client.Call<string>("getSessionID");
#elif UNITY_IOS && !UNITY_EDITOR
            return _playOnImpressionGetSessionID(client);
#elif UNITY_EDITOR
            return Guid.Empty.ToString();
#else
            return null;
#endif
        }

        public PlayOnSDK.AdUnitType GetAdType(){
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject enumAdType = client.Call<AndroidJavaObject>("getAdType");
            int typeIndex = enumAdType.Call<int> ("ordinal");
            return (PlayOnSDK.AdUnitType)typeIndex;
#elif UNITY_IOS && !UNITY_EDITOR
            return (PlayOnSDK.AdUnitType)_playOnImpressionGetAdType(client);
#elif UNITY_EDITOR
            return adUnitType;
#else
            return PlayOnSDK.AdUnitType.AudioLogoAd;
#endif


        }

        public string GetCountry(){
#if UNITY_ANDROID && !UNITY_EDITOR
            return client.Call<string>("getCountry");
#elif UNITY_IOS && !UNITY_EDITOR
            return _playOnImpressionGetCountry(client);
#elif UNITY_EDITOR
            return "None";
#else
            return null;
#endif
        }

        public double GetRevenue(){
#if UNITY_ANDROID && !UNITY_EDITOR
            return client.Call<double>("getRevenue");
#elif UNITY_IOS && !UNITY_EDITOR
            return _playOnGetRevenue(client);
#else
            return 0;
#endif
        }
    }

    public AdUnit(PlayOnSDK.AdUnitType adType)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass typeEnum = new AndroidJavaClass ("com.playon.bridge.AdUnit$AdUnitType");
        AndroidJavaObject curType = typeEnum.CallStatic<AndroidJavaObject> ("valueOf", adType.ToString ());
        client = new AndroidJavaObject ("com.playon.bridge.AdUnit", activity, curType, adListener);
#elif UNITY_IOS && !UNITY_EDITOR
        client = _playOnCreateAudioAdUnit((int)adType);
#endif
        type = adType;
        SetAdListener();
        UnityMainThreadDispatcher.Instance();
#if UNITY_EDITOR
        if (type == PlayOnSDK.AdUnitType.AudioLogoAd || type == PlayOnSDK.AdUnitType.AudioRewardedLogoAd){
            editorAdAvailable = true;
            PlayOnEditorCoroutinesManager.StartEditorCoroutine(OnAvailabilityChangeEditorCheck());
        }
#endif
    }

#if UNITY_EDITOR
    private IEnumerator OnAvailabilityChangeEditorCheck(){
        yield return new WaitForSeconds(3f);
        if(!adBlocked){
            AdCallbacks.OnAvailabilityChanged(true);
        } else
        {
            PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, "Ad overlaps parent rect. Ad blocked for showing");
        }
        yield return null;
    }
#endif

    protected void SetAdListener()
    {
#if UNITY_IOS && !UNITY_EDITOR
        adListener.adNativeListenerRef = _playOnSetListeners(client, (IntPtr)GCHandle.Alloc(adListener), AdListener.OnShowNative, AdListener.OnCloseNative, AdListener.OnClickNative, AdListener.OnAvailabilityChangedNative, AdListener.OnRewardNative, AdListener.OnImpressionNative);
#endif

        adListener.OnClose += onClose;
        adListener.OnShow += onShow;
    }

    public void ShowAd()
    {
        if(adBlocked) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        client.Call ("showAd");
#endif

#if UNITY_IOS && !UNITY_EDITOR
        _playOnShow(client);
#endif
        
#if UNITY_EDITOR
        if (type == PlayOnSDK.AdUnitType.AudioLogoAd || type == PlayOnSDK.AdUnitType.AudioRewardedLogoAd)
        {
            if (stubLogo == null)
            {
                AdCallbacks.OnAvailabilityChanged.Invoke(false);
                var logoPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PlayOn/Editor/Prefabs/PlayOnAD.prefab");
                stubLogo = Object.Instantiate(logoPrefab, Vector3.zero, Quaternion.identity);
                stubLogo.GetComponent<EditorAdUnit>().Init(this, _location, _xOffset, _yOffset, _size);
                Object.DontDestroyOnLoad(stubLogo);
                editorAdAvailable = false;
            }
        }
#endif
    }

    public void CloseAd()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        client.Call ("closeAd");
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnClose(client);
#elif UNITY_EDITOR
    if(stubLogo != null){
        stubLogo.GetComponent<EditorAdUnit>().DestroyAd();
        editorAdAvailable = true;
    }
#endif
    }

    public void SetBanner(PlayOnSDK.Position location)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass posEnum = new AndroidJavaClass ("com.playon.bridge.AdUnit$Position");
        AndroidJavaObject curPos = posEnum.CallStatic<AndroidJavaObject> ("valueOf", location.ToString ());

        client.Call ("setBanner", curPos);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetBanner(client, (int)location);
#endif
    }

    public bool IsAdAvailable()
    {
        if(adBlocked) return false;
         
#if UNITY_ANDROID && !UNITY_EDITOR
        return client.Call<bool>("isAdAvailable");
#elif UNITY_IOS && !UNITY_EDITOR
        return _playOnIsAdAvailable(client);
#elif UNITY_EDITOR
        return editorAdAvailable;
#else
        return false;
#endif
    }

    public void SetReward(PlayOnSDK.AdUnitRewardType rewardType, float value){
        this.rewardValue = value;
        this.rewardType = rewardType;
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass en = new AndroidJavaClass ("com.playon.bridge.AdUnit$RewardType");
        AndroidJavaObject curValue = en.CallStatic<AndroidJavaObject> ("valueOf", rewardType.ToString ());
        client.Call ("setReward", curValue, value);
#elif UNITY_IOS && !UNITY_EDITOR
       _playOnSetReward(client, (int)rewardType, value);
#endif
    }

    public void SetPopup(PlayOnSDK.Position position, int xOffset, int yOffset)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass en = new AndroidJavaClass ("com.playon.bridge.AdUnit$Position");
        AndroidJavaObject curValue = en.CallStatic<AndroidJavaObject> ("valueOf", position.ToString ());
        client.Call ("setPopup", curValue, xOffset, yOffset);
#elif UNITY_IOS && !UNITY_EDITOR
       _playOnSetPopup(client, (int)position, xOffset, yOffset);
#endif
    }

    public void SetProgressBar(Color progressBarColor){
#if UNITY_ANDROID && !UNITY_EDITOR
        string hexProgressBarColor = ColorUtility.ToHtmlStringRGBA(progressBarColor);
        hexProgressBarColor = "#" + hexProgressBarColor.Substring(6) + hexProgressBarColor.Remove(6);

        client.Call ("setProgressBar", hexProgressBarColor);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetProgressBar(client, "#"+ColorUtility.ToHtmlStringRGB(progressBarColor));
#endif
    }

    public void SetActionButton(PlayOnSDK.AdUnitActionButtonType actionType, float delayTime){
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass en = new AndroidJavaClass ("com.playon.bridge.AdUnit$ActionButtonType");
        AndroidJavaObject cur = en.CallStatic<AndroidJavaObject> ("valueOf", actionType.ToString ());

        client.Call ("setActionButton", cur, delayTime);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetActionButton(client, (int)actionType, delayTime);
#endif
    }
    
    public void LinkLogoToRectTransform(PlayOnSDK.Position position, RectTransform rectTransform, Canvas canvas)
    {
        if (rectTransform == null || canvas == null)
        {
            PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, "LinkLogoToRectTransform function error. RectTransform || Canvas variable is null");
            return;
        }

        Rect rect = RectTransformExtension.GetScreenRect(rectTransform, canvas);
        adBlocked = true;
        adListener.adBlocked = true;
        float bestSize = AD_SIZE_LIMIT_DP_MAX;
        Vector2 positinPX = Vector2.zero;
        for(int i = AD_SIZE_LIMIT_DP_MAX; i >= AD_SIZE_LIMIT_DP_MIN; i-=5){
            bestSize = i;
            int sizeInPX = (int)(i * PlayOnSDK.GetDeviceScale());
            positinPX = RectTransformExtension.ConvertRectToPosition(rect, position, sizeInPX);
            Rect adRect = new Rect(positinPX, new Vector2(sizeInPX, sizeInPX));
            if(RectTransformExtension.IsRectContainsRect(adRect, rect)){
                adBlocked = false;
                break;
            }
        }

        Vector2 positionDP = RectTransformExtension.PixelPositionToDP(positinPX);
        SetLogo(PlayOnSDK.Position.BottomLeft, (int)positionDP.x, (int)positionDP.y, (int)bestSize);
    }
    
    public void LinkLogoToPrefab(AdUnitAnchor adUnitAnchor)
    {
        if (adUnitAnchor == null)
        {
            PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, "LinkLogoToPrefab function error. AdUnitAnchor variable is null");
            return;
        }
        RectTransform rt = adUnitAnchor.GetRectTransform();
        Canvas canvas = adUnitAnchor.GetCanvas();
        if (canvas == null || rt == null)
        {
            PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, "LinkLogoToPrefab function error. AdUnitAnchor Integrated incorrectly");
            return;
        }
        
        Rect rect = RectTransformExtension.GetScreenRect(rt, canvas);
        float s = rt.sizeDelta.x * canvas.scaleFactor;
        Vector2 positionPX = RectTransformExtension.ConvertRectToPosition(rect, PlayOnSDK.Position.BottomLeft, (int)s);
        Vector2 positionDP = RectTransformExtension.PixelPositionToDP(positionPX);
        SetLogo(PlayOnSDK.Position.BottomLeft, (int)positionDP.x, (int)positionDP.y, (int)(s / PlayOnSDK.GetDeviceScale()));
    }

    public void SetLogo(PlayOnSDK.Position position, int xOffset, int yOffset, int size) {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass posEnum = new AndroidJavaClass ("com.playon.bridge.AdUnit$Position");
        AndroidJavaObject curPos = posEnum.CallStatic<AndroidJavaObject> ("valueOf", position.ToString ());

        client.Call ("setLogo", curPos, xOffset, yOffset, size);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetLogo(client, (int)position, xOffset, yOffset, size);
#endif

#if UNITY_EDITOR
        this._xOffset = xOffset;
        this._yOffset = yOffset;
        this._size = size;
        this._location = position;
#endif
    }
    
    public void SetVisualization(Color tint, Color background)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string hexTintColor = ColorUtility.ToHtmlStringRGBA(tint);
        hexTintColor = "#" + hexTintColor.Substring(6) + hexTintColor.Remove(6);
        string hexBackgroundColor = ColorUtility.ToHtmlStringRGBA(background);
        hexBackgroundColor = "#" + hexBackgroundColor.Substring(6) + hexBackgroundColor.Remove(6);
        client.Call ("setVisualization", hexTintColor, hexBackgroundColor);
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnSetVisualization(client, "#"+ColorUtility.ToHtmlStringRGB(tint), "#"+ColorUtility.ToHtmlStringRGB(background));
#endif
    }

    public void TrackRewardedOffer()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        client.Call ("trackRewardedOffer");
#elif UNITY_IOS && !UNITY_EDITOR
        _playOnTrackRewardedOffer(client);
#endif
    }

    void onShow()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            sceneVolumeValue = AudioListener.volume;
            AudioListener.volume = fadeValue;
        });
    }

    void onClose()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => AudioListener.volume = sceneVolumeValue);
    }

    public void Dispose(){
#if UNITY_IOS && !UNITY_EDITOR
        _playOnDestroyBridgeReference(client);
        _playOnRemoveAdUnitFromSuperView(client);
        _playOnDestroyBridgeReference(adListener.adNativeListenerRef);
        adListener = null;
#endif
    }
}