#if UNITY_EDITOR || UNITY_STANDALONE

namespace MondayOFF
{
    public static partial class EveryDay
    {
        private static void PrepareSettings(in AttAuthorizationStatus consentStatus)
        {
            Privacy.CCPA_STRING = "1---";
            Privacy.HAS_ATT_CONSENT = true;
        }
    }
}
#endif