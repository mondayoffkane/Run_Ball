using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MondayOFF {
    internal class CPButton : MonoBehaviour {
        [SerializeField] VideoPlayer _vp;
        [SerializeField] CPVideoList _videoList;
        int _mediaIndex = 0;

        private void Awake() {
            if (_videoList == null) {
                _videoList = Resources.Load("VideoList") as CPVideoList;
                Debug.Assert(_videoList != null, "[EVERYDAY] Video list Scriptable Object is NULL!");
            }

            if (_vp == null) {
                _vp = GetComponent<VideoPlayer>();
            }

            for (int i = _videoList.count - 1; i >= 0; --i) {
                var bundleID = _videoList[i].bundleID;
                if (bundleID != null && bundleID.Equals(Application.identifier)) {
                    Debug.LogWarning($"[EVERYDAY] Removing {_videoList[i].name} because it is targeting this game");
                    _videoList.list.RemoveAt(i);
                } else if (string.IsNullOrEmpty(_videoList[i].url) || _videoList[i].videoClip == null) {
                    Debug.LogWarning($"[EVERYDAY] Removing {_videoList[i].name} because it does not contain store url or video");
                    _videoList.list.RemoveAt(i);
                }
            }

            _videoList.list.Sort((a, b) => Mathf.CeilToInt(b.weight - a.weight));

            if (_videoList.count <= 0) {
                Debug.LogError("[EVERYDAY] No CP video and url found!");
                Destroy(this.gameObject);
                return;
            }

            GetComponentInChildren<Button>().onClick.AddListener(LinkToStore);
        }

        private void OnEnable() {
            ShuffleVideo();
        }

        private void OnDisable() {
            StopPlayback();
        }

        private void OnApplicationFocus(bool focusStatus) {
            if (focusStatus) {
                ShuffleVideo();
            } else {
                StopPlayback();
            }
        }

        private void StopPlayback() {
            _vp.Stop();
        }

        private void ShuffleVideo() {
            var count = _videoList.count;
            var totalWeight = 0f;
            int i = 0;
            for (; i < count; ++i) {
                totalWeight += _videoList[i].weight;
            }

            var roll = Random.Range(0f, totalWeight);

            for (i = 0; i < count; ++i) {
                var weight = _videoList[i].weight;
                if (roll < weight) {
                    _mediaIndex = i;
                    break;
                }
                roll -= weight;
            }

            _vp.clip = _videoList[_mediaIndex].videoClip;
            _vp.Play();
        }

        private void LinkToStore() {
            Application.OpenURL(_videoList[_mediaIndex].url);
        }
    }
}