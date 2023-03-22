#pragma warning disable CS0414

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace MondayOFF {
    internal class CPVideoList : ScriptableObject {
        public List<VideoAndUrl> list;
        public VideoAndUrl this[int i] => list[i];
        public int count => list.Count;
    }

    [System.Serializable]
    internal class VideoAndUrl {
        public string name = default;
        public VideoClip videoClip = default;
        public float weight = 1f;
        [SerializeField] string iosUrl = default;
        [SerializeField] string androidUrl = default;

        internal string url =>
#if UNITY_IOS
            iosUrl;
#else
            androidUrl;
#endif
        internal string bundleID {
            get {
                const string idToken = "?id=";
                var idx = androidUrl.IndexOf(idToken);
                if (idx < 0){
                    return null;
                }
                idx += idToken.Length;
                return androidUrl.Substring(idx, androidUrl.Length - idx);
            }
        }

#if UNITY_EDITOR
        internal void SetData(string name, string iosUrl, string androidUrl) {
            this.name = name;
            this.iosUrl = iosUrl;
            this.androidUrl = androidUrl;
        }
#endif

    }
}

#pragma warning restore CS0414