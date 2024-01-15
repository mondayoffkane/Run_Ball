using System;
using Adverty.Native;
#if !UNITY_EDITOR && UNITY_ANDROID
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class AndroidRenderingPlugin : IAndroidRenderingBridge
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        [DllImport("AdvertyAndroidPlugin")]
        private static extern void SetNativeTexture(IntPtr handler, int width, int height, int id, int scale);

        [DllImport("AdvertyAndroidPlugin")]
        private static extern void DestroyNativeTexture(int textureId);

        [DllImport("AdvertyAndroidPlugin")]
        private static extern IntPtr GetWebViewRenderEventFunc();

        [DllImport("AdvertyAndroidPlugin")]
        private static extern int GetWebViewGraphicsEventID();
#endif

        public void SendTexture(IntPtr handler, int width, int height, int id, int scale)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            SetNativeTexture(handler, width, height, id, scale);
#endif
        }

        public void DestroyTexture(int id)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            DestroyNativeTexture(id);
#endif
        }

        public IntPtr GetRenderEventFunction()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return GetWebViewRenderEventFunc();
#else
            return IntPtr.Zero;
#endif
        }

        public int GetRenderEventID()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return GetWebViewGraphicsEventID();
#else
            return -1;
#endif
        }
    }
}
