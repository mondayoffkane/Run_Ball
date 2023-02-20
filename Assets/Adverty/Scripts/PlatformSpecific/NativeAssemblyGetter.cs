using UnityEngine;

namespace Adverty.PlatformSpecific
{
    internal class NativeAssemblyGetter
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            AdvertySettings.NativeAssembly = typeof(NativeAssemblyGetter).Assembly;
        }
    }
}
