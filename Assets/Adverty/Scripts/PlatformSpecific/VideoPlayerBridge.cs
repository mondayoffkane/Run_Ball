using System;

#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif
using Adverty.Native;

namespace Adverty.PlatformSpecific
{
    public class VideoPlayerBridge : IVASTPlayerBridge
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        protected const string VIDEO_LIBRARY_NAME = "AdvertyVideoPlayer";
        protected const string LIBRARY_NAME = "AdvertyOSXPlugin";
#elif UNITY_ANDROID
        protected const string VIDEO_LIBRARY_NAME = "AdvertyVideoPlayer";
        protected const string LIBRARY_NAME = "AdvertyAndroidPlugin";
#elif UNITY_IOS
        protected const string VIDEO_LIBRARY_NAME = "__Internal";
        protected const string LIBRARY_NAME = "__Internal";
#endif

#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern IntPtr CreateVideoPlayer(VideoPlayerConfig config);

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern void SetVideoTexture(IntPtr videoPlayer, IntPtr texture, int width, int heigth, int format);

        [DllImport(VIDEO_LIBRARY_NAME, CharSet = CharSet.Ansi)]
        public static extern void LoadVideo(IntPtr videoPlayer, string vastString);

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern bool IsVideoPlaying(IntPtr videoPlayer);

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern void PlayVideo(IntPtr videoPlayer);

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern void StopVideo(IntPtr videoPlayer);

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern void PauseVideo(IntPtr videoPlayer);

        [DllImport(VIDEO_LIBRARY_NAME)]
        public static extern void DestroyVideoPlayer(IntPtr videoPlayer);

        [DllImport(LIBRARY_NAME)]
        public static extern IntPtr GetVideoRenderEventFunc();

        [DllImport(LIBRARY_NAME)]
        public static extern int GetVideoPlayerGraphicsEventID();
#endif

        public IntPtr CreatePlayer(VideoPlayerConfig videoPlayerConfig)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            return CreateVideoPlayer(videoPlayerConfig);
#else
            return IntPtr.Zero;
#endif
        }

        public void Destroy(IntPtr videoPlayer)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            DestroyVideoPlayer(videoPlayer);
#endif
        }

        public void LoadVASTVideo(IntPtr videoPlayer, string vastString)
        {
#if(UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            LoadVideo(videoPlayer, vastString);
#endif
        }

        public bool IsPlaying(IntPtr videoPlayer)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            return IsVideoPlaying(videoPlayer);
#else
            return false;
#endif
        }

        public void Play(IntPtr videoPlayer)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            PlayVideo(videoPlayer);
#endif

        }

        public void Pause(IntPtr videoPlayer)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            PauseVideo(videoPlayer);
#endif

        }

        public void Stop(IntPtr videoPlayer)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            StopVideo(videoPlayer);
#endif

        }

        public void SetTexture(IntPtr videoPlayer, IntPtr texture, int width, int height, int format)
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            SetVideoTexture(videoPlayer, texture, width, height, format);
#endif

        }

        public IntPtr GetRenderUpdateFuncPtr()
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            return GetVideoRenderEventFunc();
#else
            return IntPtr.Zero;
#endif
        }

        public int GetGraphicsEventID()
        {
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            return GetVideoPlayerGraphicsEventID();
#else
            return -1;
#endif
        }
    }
}
