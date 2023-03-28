using System;
using Adverty.Native;
#if !UNITY_EDITOR && UNITY_ANDROID
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class AndroidNativeUtils : NativeBridge, IAndroidNativeUtils
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertyRequestAdIdData(IntPtr callback);

        [DllImport(LIBRARY_NAME)]
        private static extern NativeIABData AdvertyRequestIABData();
#endif
        public void RequestAdIdData(IntPtr callback)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            AdvertyRequestAdIdData(callback);
#endif
        }

        public NativeIABData RequestIABData()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return AdvertyRequestIABData();
#else
            return new NativeIABData();
#endif
        }
    }
}
