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
        private static extern IntPtr create(IntPtr cbo, int width, int height, int timeout, int scale);

        [DllImport(LIBRARY_NAME)]
        private static extern void setFramerate(IntPtr ptr, int framesPerSecond);

        [DllImport(LIBRARY_NAME)]
        private static extern void setDrawAfterScreenUpdates(IntPtr ptr, bool afterScreenUpdates);

        [DllImport(LIBRARY_NAME)]
        private static extern void viewabilityCheck(IntPtr ptr, IntPtr checkPoints, IntPtr viewabilityCallback);

        public IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale)
        {
            return create(cbo, width, height, loadTimeout, scale);
        }

        public void SetDrawScreenAfterUpdate(IntPtr ptr, bool afterScreenUpdate)
        {
            setDrawAfterScreenUpdates(ptr, afterScreenUpdate);
        }

        public void SetFramerate(IntPtr ptr, int framesPerSecond)
        {
            setFramerate(ptr, framesPerSecond);
        }

        public void ViewabilityCheck(IntPtr ptr, IntPtr checkPoints, IntPtr viewabilityCallback)
        {
            viewabilityCheck(ptr, checkPoints, viewabilityCallback);
        }
#else
        public override IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale)
        {
            return IntPtr.Zero;
        }

        public void SetDrawScreenAfterUpdate(IntPtr ptr, bool afterScreenUpdate)
        {
        }

        public void SetFramerate(IntPtr ptr, int framesPerSecond)
        {
        }
        public void ViewabilityCheck(IntPtr ptr, IntPtr checkPoints, IntPtr viewabilityCallback)
        {
        }
#endif
    }
}
