
namespace Adverty.PlatformSpecific
{
    public class NativeBridge
    {
#if UNITY_ANDROID
        protected const string LIBRARY_NAME = "glbridge";
#elif UNITY_IOS
        protected const string LIBRARY_NAME = "__Internal";
#endif
    }
}
