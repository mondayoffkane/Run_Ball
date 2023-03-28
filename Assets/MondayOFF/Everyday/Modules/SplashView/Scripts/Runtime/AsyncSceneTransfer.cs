using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MondayOFF {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal class ScenePickerAttribute : PropertyAttribute {
        internal ScenePickerAttribute() { }
    }

    [DisallowMultipleComponent]
    internal class AsyncSceneTransfer : MonoBehaviour {
        [SerializeField][ScenePicker] int _targetSceneIndex = 0;
        AsyncOperation _asyncOp = null;

        public void Transfer() {
            if (_asyncOp == null) {
                return;
            }
            _asyncOp.allowSceneActivation = true;
        }

        private void LoadSceneAsync() {
            _asyncOp = SceneManager.LoadSceneAsync(_targetSceneIndex);
            if (_asyncOp == null) {
                Debug.LogWarning($"Failed to find scene index {_targetSceneIndex}");
                return;
            }
            _asyncOp.allowSceneActivation = false;
        }

        private void Start() {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            if (_targetSceneIndex < 0 || _targetSceneIndex >= sceneCount) {
                Debug.LogException(new System.Exception("Invalid scene is selected!"));
                return;
            }

            LoadSceneAsync();
        }
    }
}