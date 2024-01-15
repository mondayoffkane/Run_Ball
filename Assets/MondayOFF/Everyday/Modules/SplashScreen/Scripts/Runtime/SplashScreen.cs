using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MondayOFF
{
    internal class SplashScreen : MonoBehaviour
    {
        [SerializeField] EverydaySettings _settings = default;
        [SerializeField] Image _mondayoffLogoImage = default;
        [SerializeField] Image _crossImage = default;
        [SerializeField] Image _companyLogoImage = default;

        public void TransferToGameScene()
        {
            SceneManager.LoadScene(_settings.gameSceneName);
        }

        private void Awake()
        {
            if (_settings.companyLogo != null)
            {
                _companyLogoImage.sprite = _settings.companyLogo;
                return;
            }

            _crossImage.enabled = _companyLogoImage.enabled = false;
            _mondayoffLogoImage.rectTransform.anchoredPosition = new Vector2(0f, -_mondayoffLogoImage.rectTransform.sizeDelta.y / 2f);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var assets = Resources.LoadAll<EverydaySettings>("EverydaySettings");
            if (assets == null || assets.Length <= 0)
            {
                Debug.Log("NOT found, search all");
                assets = Resources.LoadAll<EverydaySettings>("");
            }
            if (assets.Length != 1)
            {
                EverydayLogger.Error($"Found 0 or multiple {typeof(EverydaySettings).Name}s in Resources folder. There should only be one.");
            }
            else
            {
                _settings = assets[0];
            }
            Debug.Assert(_settings != null, "NO SETTINGS FOUND");

            if (_settings.companyLogo != null)
            {
                _companyLogoImage.sprite = _settings.companyLogo;
                return;
            }
        }
#endif
    }
}