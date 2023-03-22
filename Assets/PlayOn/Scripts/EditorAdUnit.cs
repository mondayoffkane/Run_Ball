#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class EditorAdUnit : MonoBehaviour
{
    private Text _timer;
    public int playLength = 8;
    public RectTransform rect;
    public Canvas canvas;
    private int _xOffset;
    private int _yOffset;
    private Vector2 _size;
    private PlayOnSDK.Position _location;
    private AdUnit _adUnit;
    private Button _btn;
    private EditorPopUp _popUp;
    private static string AD_POPUP_PREFAB_FILENAME = "PlayOnAdPopup.prefab";
    
    public void Init (AdUnit adUnit, PlayOnSDK.Position location, int xOffset, int yOffset, Vector2 size) {
        _location = location;
        _adUnit = adUnit;
        _xOffset = xOffset;
        _yOffset = yOffset;
        _size = size;
        _timer = this.GetComponentInChildren<Text>();
        _btn = this.GetComponentInChildren<Button>();
        _btn.onClick.AddListener(OnAdClicked);
        StartCoroutine(Timer());
        SetPosition();
#if UNITY_EDITOR
        _adUnit.AdCallbacks.OnShow();
        _adUnit.AdCallbacks.OnImpression(new AdUnit.ImpressionData(_adUnit.type));
#endif
        if (_adUnit.type == PlayOnSDK.AdUnitType.AudioRewardedBannerAd || _adUnit.type == PlayOnSDK.AdUnitType.AudioRewardedLogoAd)
        {
            if (_adUnit.rewardType == PlayOnSDK.AdUnitRewardType.EndLevel)
            {
                _adUnit.AdCallbacks.OnReward.Invoke(_adUnit.rewardValue);
            }
        }
    }

    private IEnumerator Timer()
    {
        int time = playLength;
        while (time > 0)
        {
            _timer.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        if (_adUnit.type == PlayOnSDK.AdUnitType.AudioRewardedBannerAd || _adUnit.type == PlayOnSDK.AdUnitType.AudioRewardedLogoAd)
        {
            if (_adUnit.rewardType == PlayOnSDK.AdUnitRewardType.InLevel)
            {
                _adUnit.AdCallbacks.OnReward.Invoke(_adUnit.rewardValue);
            }
        }
        DestroyAd();
        yield return null;
    }

    public void DestroyAd() {
        _adUnit.editorAdAvailable = true;
        _adUnit.AdCallbacks.OnClose.Invoke();
        _adUnit.AdCallbacks.OnAvailabilityChanged.Invoke(true);
        StopAllCoroutines();
        if (_popUp) Destroy(_popUp.gameObject);
        _popUp = null;
        Destroy(gameObject);
    }

    private void OnAdClicked()
    {
        _adUnit.AdCallbacks.OnClick.Invoke();
    }

    private void SetPosition() {
        float optimalDPI = PlayOnSDK.GetUnityEditorDPI();
        if (!PlayOnSDK.DPISettedByUser())
        {
            optimalDPI = GetOptimalDPI();
        }
        
        if (_adUnit.type == PlayOnSDK.AdUnitType.AudioRewardedBannerAd || _adUnit.type == PlayOnSDK.AdUnitType.AudioRewardedLogoAd)
        {
            string prefabPath = EditorHelper.GetAssetBasedPath(AD_POPUP_PREFAB_FILENAME);
            if (string.IsNullOrEmpty(prefabPath))
            {
                PlayOnSDK.LogE(PlayOnSDK.LogLevel.Debug, "Can't find " + AD_POPUP_PREFAB_FILENAME + " asset");
                return;
            }
            
            EditorPopUp logoPrefab = AssetDatabase.LoadAssetAtPath<EditorPopUp>(prefabPath);
            _popUp = Instantiate(logoPrefab, Vector3.zero, Quaternion.identity);
            
            if(_adUnit.rewardType == PlayOnSDK.AdUnitRewardType.EndLevel) 
                _popUp.ShowPopUp(EditorPopUpType.Banner, PlayOnSDK.Position.BottomCenter, 0, 0, optimalDPI);
            else 
                _popUp.ShowPopUp(EditorPopUpType.Logo, _adUnit.popUpPosition, _adUnit.popUpOffsetX, _adUnit.popUpOffsetX, optimalDPI);
            
            DontDestroyOnLoad(_popUp);
        }
        
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2((_size.x + 0.5f) * (optimalDPI / 160f) / canvas.scaleFactor, (_size.y + 0.5f) * (optimalDPI / 160f) / canvas.scaleFactor);
        var xPos = (_xOffset * (optimalDPI / 160f)) + 0.5f;
        var yPos = (_yOffset * (optimalDPI / 160f) + 0.5f);
        switch (_location)
        {
            case PlayOnSDK.Position.Centered:
                rect.pivot = new Vector2(0.5f, 0.5f);
                yPos = canvas.pixelRect.height / 2 - yPos;
                xPos = canvas.pixelRect.width / 2 - xPos;
                break;
            case PlayOnSDK.Position.BottomLeft:
                rect.pivot = Vector2.zero;
                break;
            case PlayOnSDK.Position.BottomRight:
                rect.pivot = new Vector2(1f, 0f);
                xPos = canvas.pixelRect.width - xPos;
                break;
            case PlayOnSDK.Position.TopLeft:
                rect.pivot = new Vector2(0f, 1f);
                yPos = canvas.pixelRect.height - yPos;
                break;
            case PlayOnSDK.Position.TopRight:
                rect.pivot = new Vector2(1f, 1f);
                yPos = canvas.pixelRect.height - yPos;
                xPos = canvas.pixelRect.width - xPos;
                break;
            case PlayOnSDK.Position.CenterLeft:
                rect.pivot = new Vector2(0f, 0.5f);
                yPos = canvas.pixelRect.height / 2 - yPos;
                break;
            case PlayOnSDK.Position.CenterRight:
                rect.pivot = new Vector2(1f, 0.5f);
                yPos = canvas.pixelRect.height / 2 - yPos;
                xPos = canvas.pixelRect.width - xPos;
                break;
            case PlayOnSDK.Position.BottomCenter:
                rect.pivot = new Vector2(0.5f, 0f);
                xPos = canvas.pixelRect.width / 2 - xPos;
                break;
            case PlayOnSDK.Position.TopCenter:
                rect.pivot = new Vector2(0.5f, 1f);
                xPos = canvas.pixelRect.width / 2 - xPos;
                yPos = canvas.pixelRect.height - yPos;
                break;
        }
        rect.position = new Vector3(xPos, yPos, 0);
    }

    private float GetOptimalDPI() {
        float result = 0;
        if (canvas.pixelRect.width >= 1440)
        {
            result = 440;
        }
        else if (canvas.pixelRect.width >= 1080)
        {
            result = 323;
        }
        else if (canvas.pixelRect.width >= 720)
        {
            result = 252;
        }
        else if (canvas.pixelRect.width >= 480)
        {
            result = 170;
        }

        return result;
    }
}
#endif