using UnityEngine;
using UnityEngine.UI;

namespace MondayOFF {
    [RequireComponent(typeof(CanvasGroup), typeof(Animator))]
    internal class EverydaySplash : MonoBehaviour {
        [SerializeField] AsyncSceneTransfer _sceneTransfer = default;
        [SerializeField] CanvasGroup _canvasGroup = default;
        [SerializeField] Animator _fadeAnimator = default;
        [SerializeField] Image _logoImage = default;

        private void OnFadeComplete() {
            _sceneTransfer.Transfer();
        }

        private void DestroyCanvas() {
            Destroy(_sceneTransfer.gameObject);
            Resources.UnloadUnusedAssets();
        }

        private void Awake() {
            DontDestroyOnLoad(_sceneTransfer.gameObject);
        }

#if UNITY_EDITOR
        private void OnValidate() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _sceneTransfer = GetComponentInParent<AsyncSceneTransfer>();
            _fadeAnimator = GetComponent<Animator>();

            _logoImage = GetComponentInChildren<Image>();
        }
#endif
    }
}