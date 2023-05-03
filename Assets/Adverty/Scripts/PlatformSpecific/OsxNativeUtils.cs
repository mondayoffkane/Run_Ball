#if UNITY_EDITOR_OSX || (!UNITY_EDITOR && UNITY_STANDALONE_OSX)
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class OsxUtils : IOsxNativeUtils
    {
#if UNITY_EDITOR_OSX || (!UNITY_EDITOR && UNITY_STANDALONE_OSX)
        [DllImport("utils", CharSet = CharSet.Unicode)]
        private static extern string _Utils_GetSystemLocale();

        public string GetSystemLocale()
        {
            return _Utils_GetSystemLocale();
        }
#else
        public string GetSystemLocale()
        {
            return null;
        }
#endif
    }
}
