using Adverty.Native.Interface;

#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.iOS;
#endif

namespace Adverty.PlatformSpecific
{
    internal class IosSystemVersion : ISystemVersion
    {
        public string GetSystemVersion()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return Device.systemVersion;
#endif
            return null;
        }
    }
}
