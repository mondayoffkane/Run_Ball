using System;
using Adverty.Native;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
namespace Adverty.PlatformSpecific
{
    public class IosWebViewTextureBridge : BaseWebViewTextureBridge, IIosWebViewTextureBridge
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport(LIBRARY_NAME)]
        private static extern IntPtr create(IntPtr cbo, int width, int height, int timeout, int scale, bool showDebugView);

        [DllImport(LIBRARY_NAME)]
        private static extern void setFramerate(IntPtr ptr, int framesPerSecond);

        [DllImport(LIBRARY_NAME)]
        private static extern void setDrawAfterScreenUpdates(IntPtr ptr, bool afterScreenUpdates);

        [DllImport(LIBRARY_NAME)]
        private static extern void viewabilityCheck(IntPtr ptr, IntPtr checkPoints, IntPtr viewabilityCallback);
#endif
        public override IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale, bool showDebugView)
        {
#if UNITY_IOS && !UNITY_EDITOR
            return create(cbo, width, height, loadTimeout, scale, showDebugView);
#else
            return IntPtr.Zero;
#endif
        }

        public void SetDrawScreenAfterUpdate(IntPtr ptr, bool afterScreenUpdate)
        {
#if UNITY_IOS && !UNITY_EDITOR
            setDrawAfterScreenUpdates(ptr, afterScreenUpdate);
#endif
        }

        public void SetFramerate(IntPtr ptr, int framesPerSecond)
        {
#if UNITY_IOS && !UNITY_EDITOR
            setFramerate(ptr, framesPerSecond);
#endif
        }

        public void ViewabilityCheck(IntPtr ptr, IntPtr checkPoints, IntPtr viewabilityCallback)
        {
#if UNITY_IOS && !UNITY_EDITOR
            viewabilityCheck(ptr, checkPoints, viewabilityCallback);
#endif
        }
    }
}
