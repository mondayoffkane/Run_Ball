using System;
using Adverty.Native;
#if UNITY_ANDROID && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class AndroidWebViewTextureBridge : BaseWebViewTextureBridge, IAndroidWebViewTextureBridge
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        [DllImport(LIBRARY_NAME)]
        private static extern IntPtr getCustomRenderEventFunc();

        [DllImport(LIBRARY_NAME)]
        private static extern void setOnPageStarted(IntPtr ptr, IntPtr onPageStartedDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void setOnCreated(IntPtr ptr, IntPtr onCreatedDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern IntPtr create(int cbo, int width, int height, int loadTimeout, int scale);

        public IntPtr GetCustomRenderEventFunc()
        {
            return getCustomRenderEventFunc();
        }

        public override IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale)
        {
            return create(cbo.ToInt32(), width, height, loadTimeout, scale);
        }

        public void SetOnCreate(IntPtr ptr, IntPtr onCreateDelegate)
        {
            setOnCreated(ptr, onCreateDelegate);
        }

        public void SetOnPageStarted(IntPtr ptr, IntPtr onPageStartedDelegate)
        {
            setOnPageStarted(ptr, onPageStartedDelegate);
        }
#else
        public override IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale)
        {
            return IntPtr.Zero;
        }

        public IntPtr GetCustomRenderEventFunc()
        {
            return IntPtr.Zero;
        }

        public void SetOnCreate(IntPtr ptr, IntPtr onCreateDelegate)
        {
        }

        public void SetOnPageStarted(IntPtr ptr, IntPtr onPageStartedDelegate)
        {
        }
#endif
    }
}
