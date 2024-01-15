using System;
using Adverty.Native;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class BaseWebViewTextureBridge : NativeBridge, IWebViewTextureBridge
    {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

        [DllImport(LIBRARY_NAME)]
        private static extern IntPtr destroy(IntPtr ptr, string data);

        [DllImport(LIBRARY_NAME)]
        private static extern void loadData(IntPtr ptr, string data, string baseUrl);

        [DllImport(LIBRARY_NAME)]
        private static extern void loadUrl(IntPtr ptr, string url);

        [DllImport(LIBRARY_NAME)]
        private static extern bool setOnClickAction(IntPtr ptr, IntPtr onClickDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void setOnPageFinished(IntPtr ptr, IntPtr onPageFinishedDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void setOnReceivedError(IntPtr ptr, IntPtr onReceivedErrorDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void setOnTextureCheckPassed(IntPtr ptr, IntPtr onTextureCheckCompleteDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void setRenderingActive(IntPtr ptr, bool active);

        [DllImport(LIBRARY_NAME)]
        private static extern void setFramerate(IntPtr ptr, int framesPerSecond);

        [DllImport(LIBRARY_NAME)]
        private static extern void setOnHtmlLoadTimeout(IntPtr ptr, IntPtr onHtmlLoadTimeoutDelegate);

        [DllImport(LIBRARY_NAME)]
        private static extern void touch(IntPtr ptr, float x, float y);

        [DllImport(LIBRARY_NAME)]
        private static extern void sendViewabilityData(IntPtr ptr, string data);

        [DllImport(LIBRARY_NAME)]
        private static extern void triggerViewedImpression(IntPtr ptr, string data);

        [DllImport(LIBRARY_NAME)]
        private static extern bool setOnCreated(IntPtr ptr, IntPtr onCreatedDelegate);
#endif
        public virtual IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale, bool showDebugView)
        {
            return IntPtr.Zero;
        }

        public void Destroy(IntPtr ptr, string data)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
             destroy(ptr, data);
#endif
        }

        public void LoadData(IntPtr ptr, string data, string baseUrl)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            loadData(ptr, data, baseUrl);
#endif
        }

        public void LoadURL(IntPtr ptr, string url)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            loadUrl(ptr, url);
#endif
        }

        public void SendViewabilityData(IntPtr ptr, string data)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            sendViewabilityData(ptr, data);
#endif
        }

        public void SetOnCreated(IntPtr ptr, IntPtr onCreateDelegate)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setOnCreated(ptr, onCreateDelegate);
#endif
        }

        public void SetOnClicked(IntPtr ptr, IntPtr onClickedDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setOnClickAction(ptr, onClickedDelegate);
#endif
        }

        public void SetOnHtmlLoadTimeout(IntPtr ptr, IntPtr onHtmlLoadTimeoutDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setOnHtmlLoadTimeout(ptr, onHtmlLoadTimeoutDelegate);
#endif
        }

        public void SetOnPageFinished(IntPtr ptr, IntPtr onPageFinishedDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setOnPageFinished(ptr, onPageFinishedDelegate);
#endif
        }

        public void SetOnReceiveError(IntPtr ptr, IntPtr onReceivedErrorDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setOnReceivedError(ptr, onReceivedErrorDelegate);
#endif
        }

        public void SetOnTextureCheckPassed(IntPtr ptr, IntPtr onTextureCheckPassedDelegate)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setOnTextureCheckPassed(ptr, onTextureCheckPassedDelegate);
#endif
        }

        public void SetRenderActive(IntPtr ptr, bool active)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            setRenderingActive(ptr, active);
#endif
        }

        public void Touch(IntPtr ptr, float x, float y)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            touch(ptr, x, y);
#endif
        }

        public void TriggerViewedImpression(IntPtr ptr, string data)
        {
#if(UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            triggerViewedImpression(ptr, data);
#endif
        }
    }
}
