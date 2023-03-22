using System;
using Adverty.Native;
#if !UNITY_EDITOR && UNITY_ANDROID
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class AndroidRenderingPlugin : IAndroidRenderingPlugin
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        [DllImport("glbridge")]
        private static extern void SetGlIssuePluginEventMethod(IntPtr glIssuePluginMethod, int renderMode);

        [DllImport("glbridge")]
        private static extern void IssuePluginEvent(int eventId);

        [DllImport("glbridge")]
        private static extern void CustomRenderEvent(int eventId);

        [DllImport("glbridge")]
        private static extern void SetNativeTexture(IntPtr handler, int width, int height, int id, int scale);

        [DllImport("glbridge")]
        private static extern void DestroyNativeTexture(int textureId);

        public void NativeSetGlIssuePluginEventMethod(IntPtr glIssuePluginMethod, int renderMode)
        {
            SetGlIssuePluginEventMethod(glIssuePluginMethod, renderMode);
        }

        public void NativeIssuePluginEvent(int eventId)
        {
            IssuePluginEvent(eventId);
        }

        public void Render(int eventId)
        {
            CustomRenderEvent(eventId);
        }

        public void SendTexture(IntPtr handler, int width, int height, int id, int scale)
        {
            SetNativeTexture(handler, width, height, id, scale);
        }

        public void DestroyTexture(int id)
        {
            DestroyNativeTexture(id);
        }
#else
        public void NativeSetGlIssuePluginEventMethod(IntPtr glIssuePluginMethod, int renderMode)
        {
        }

        public void NativeIssuePluginEvent(int eventId)
        {
        }

        public void Render(int eventId)
        {
        }

        public void SendTexture(IntPtr handler, int width, int height, int id, int scale)
        {
        }

        public void DestroyTexture(int id)
        {
        }
#endif
    }
}
