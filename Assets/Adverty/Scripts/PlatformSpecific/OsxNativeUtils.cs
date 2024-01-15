#if UNITY_EDITOR_OSX || (!UNITY_EDITOR && UNITY_STANDALONE_OSX)
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class OsxUtils : IOsxNativeUtils
    {
#if UNITY_EDITOR_OSX || (!UNITY_EDITOR && UNITY_STANDALONE_OSX)
        [DllImport("AdvertyOSXPlugin")]
        private static extern string GetLocale();

        public string GetSystemLocale()
        {
            return GetLocale();
        }
#else
        public string GetSystemLocale()
        {
            return null;
        }
#endif
    }
}
