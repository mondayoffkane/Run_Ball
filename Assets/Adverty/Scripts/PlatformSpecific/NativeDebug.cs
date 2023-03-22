using Adverty.Native;
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class NativeDebug : NativeBridge, INativeDebug
    {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        [DllImport(LIBRARY_NAME)]
        private static extern void AdvertyInitializeDebug();
#endif

        public void Initialize()
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            AdvertyInitializeDebug();
#endif
        }
    }
}
