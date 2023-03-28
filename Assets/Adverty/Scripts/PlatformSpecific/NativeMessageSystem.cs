using System;
using Adverty.Native;
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class NativeMessageSystem : NativeBridge, INativeMessageSystem
    {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertySetNativeEventsCallback(IntPtr callbackDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertySetNativeEventsCallbackWithData(IntPtr callbackDelegate);
#endif

        public void SetNativeEventsCallback(IntPtr callbackDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            AdvertySetNativeEventsCallback(callbackDelegate);
#endif
        }

        public void SetNativeEventsCallbackWithData(IntPtr callbackDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            AdvertySetNativeEventsCallbackWithData(callbackDelegate);
#endif
        }
    }
}
