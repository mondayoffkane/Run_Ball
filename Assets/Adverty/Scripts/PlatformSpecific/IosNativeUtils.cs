using System;
using Adverty.Native;
#if !UNITY_EDITOR && UNITY_IOS
using System.Runtime.InteropServices;
#endif
namespace Adverty.PlatformSpecific
{
    public class IosNativeUtils : NativeBridge, IIosNativeUtils
    {
#if !UNITY_EDITOR && UNITY_IOS
        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertyUtilsSetUnityEventListenerId(int listenerId);

        [DllImport(LIBRARY_NAME)]
        private static extern string AdvertyUtilsGetSystemLocale();

        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertyUtilsGetUserAgent();

        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertyRequestAdIdData(IntPtr callback);

        [DllImport(LIBRARY_NAME)]
        private static extern NativeIABData AdvertyRequestIABData();
#endif

        public string GetSystemLocale()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return AdvertyUtilsGetSystemLocale();
#else
            return string.Empty;
#endif
        }

        public void SetUnityEventListenerId(int listenerId)
        {
#if !UNITY_EDITOR && UNITY_IOS
            AdvertyUtilsSetUnityEventListenerId(listenerId);
#endif
        }

        public void GetUserAgent()
        {
#if !UNITY_EDITOR && UNITY_IOS
            AdvertyUtilsGetUserAgent();
#endif
        }

        public void RequestAdIdData(IntPtr callback)
        {
#if !UNITY_EDITOR && UNITY_IOS
            AdvertyRequestAdIdData(callback);
#endif
        }

        public NativeIABData RequestIABData()
        {
#if !UNITY_EDITOR && UNITY_IOS
            return AdvertyRequestIABData();
#else
            return new NativeIABData();
#endif
        }
    }
}
