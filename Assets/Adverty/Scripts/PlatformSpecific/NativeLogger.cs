using System;
using Adverty.Native;
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class NativeLogger : NativeBridge, INativeLogger
    {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertySetNativeDebugCallback(IntPtr callbackDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertySetNativeDebugWithStackTraceCallback(IntPtr callbackDelegate);
#endif

        public void SetNativeDebugCallback(IntPtr callbackDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            AdvertySetNativeDebugCallback(callbackDelegate);
#endif
        }

        public void SetNativeDebugWithStackTraceCallback(IntPtr callbackDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            AdvertySetNativeDebugWithStackTraceCallback(callbackDelegate);
#endif
        }
    }
}
