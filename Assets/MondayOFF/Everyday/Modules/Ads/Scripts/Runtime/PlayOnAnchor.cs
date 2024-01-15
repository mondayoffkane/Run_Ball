using UnityEngine;

namespace MondayOFF {
    [RequireComponent(typeof(RectTransform))]
    public class PlayOnAnchor : MonoBehaviour {
        [SerializeField] PlayOnSDK.Position _anchor = default;
        private RectTransform _rt = default;
        private Canvas _canvas = default;

        private void Awake() {
            _rt = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null) {
                EverydayLogger.Error("PlayOnAnchor must be a descendant of Canvas!");
            }
        }

        private void Start() {
            if (AdsManager.LinkLogoToRectTransform(_anchor, _rt, _canvas)) {
                // this.enabled = false;
                Destroy(this);
            } else {
                PlayOnSDK.OnInitializationFinished -= LinkLogoToRectTransform;
                PlayOnSDK.OnInitializationFinished += LinkLogoToRectTransform;
            }
        }

        private void LinkLogoToRectTransform() {
            AdsManager.LinkLogoToRectTransform(_anchor, _rt, _canvas);
            Destroy(this);
        }

        private void OnDisable() {
            PlayOnSDK.OnInitializationFinished -= LinkLogoToRectTransform;
        }
    }
}