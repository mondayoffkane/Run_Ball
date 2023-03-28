using System;
using Adverty.Native;
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class NativeRedirectionDialogBridge : NativeBridge, INativeRedirectionDialog
    {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertyShowRedirectionDialog(string url, IntPtr callback);
#endif
        public void ShowRedirectionDialog(string url, IntPtr callback)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            AdvertyShowRedirectionDialog(url, callback);
#endif
        }
    }
}
