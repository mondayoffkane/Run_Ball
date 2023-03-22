using System;
using Adverty.Native;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class BaseWebViewTextureBridge : NativeBridge, IWebViewTextureBridge
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

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

        public virtual IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale)
        {
            return IntPtr.Zero;
        }

        public void Destroy(IntPtr ptr, string data)
        {
            destroy(ptr, data);
        }

        public void LoadData(IntPtr ptr, string data, string baseUrl)
        {
            loadData(ptr, data, baseUrl);
        }

        public void LoadURL(IntPtr ptr, string url)
        {
            loadUrl(ptr, url);
        }

        public void SetOnClicked(IntPtr ptr, IntPtr onClickedDelegate)
        {
            setOnClickAction(ptr, onClickedDelegate);
        }

        public void SetOnHtmlLoadTimeout(IntPtr ptr, IntPtr onHtmlLoadTimeoutDelegate)
        {
            setOnHtmlLoadTimeout(ptr, onHtmlLoadTimeoutDelegate);
        }

        public void SetOnPageFinished(IntPtr ptr, IntPtr onPageFinishedDelegate)
        {
            setOnPageFinished(ptr, onPageFinishedDelegate);
        }

        public void SetOnReceiveError(IntPtr ptr, IntPtr onReceivedErrorDelegate)
        {
            setOnReceivedError(ptr, onReceivedErrorDelegate);
        }

        public void SetOnTextureCheckPassed(IntPtr ptr, IntPtr onTextureCheckPassedDelegate)
        {
            setOnTextureCheckPassed(ptr, onTextureCheckPassedDelegate);
        }

        public void SetRenderActive(IntPtr ptr, bool active)
        {
            setRenderingActive(ptr, active);
        }

        public void Touch(IntPtr ptr, float x, float y)
        {
            touch(ptr, x, y);
        }

        public void SendViewabilityData(IntPtr ptr, string data)
        {
            sendViewabilityData(ptr, data);
        }

        public void TriggerViewedImpression(IntPtr ptr, string data)
        {
            triggerViewedImpression(ptr, data);
        }
#else
        public virtual IntPtr Create(IntPtr cbo, int width, int height, int loadTimeout, int scale)
        {
            return IntPtr.Zero;
        }

        public void Destroy(IntPtr ptr, string data)
        {
        }

        public void LoadData(IntPtr ptr, string data, string baseUrl)
        {
        }

        public void LoadURL(IntPtr ptr, string url)
        {
        }

        public void SetOnClicked(IntPtr ptr, IntPtr onClickedDelegate)
        {
        }

        public void SetOnHtmlLoadTimeout(IntPtr ptr, IntPtr onHtmlLoadTimeoutDelegate)
        {
        }

        public void SetOnPageFinished(IntPtr ptr, IntPtr onPageFinishedDelegate)
        {
        }

        public void SetOnReceiveError(IntPtr ptr, IntPtr onReceivedErrorDelegate)
        {
        }

        public void SetOnTextureCheckPassed(IntPtr ptr, IntPtr onTextureCheckPassedDelegate)
        {
        }

        public void SetRenderActive(IntPtr ptr, bool active)
        {
        }

        public void Touch(IntPtr ptr, float x, float y)
        {
        }

        public void SendViewabilityData(IntPtr ptr, string data)
        {
        }

        public void TriggerViewedImpression(IntPtr ptr, string data)
        {
        }
#endif
    }
}
