#if !UNITY_EDITOR && !UNITY_STANDALONE
using UnityEngine;

namespace MondayOFF {
    public static partial class EveryDay {
        private static void PrepareSettings(in AttAuthorizationStatus consentStatus) {
            Privacy.HAS_ATT_CONSENT = consentStatus == AttAuthorizationStatus.Authorized;
            // MAX
            MaxSdk.SetHasUserConsent(Privacy.HAS_ATT_CONSENT);
            MaxSdk.SetDoNotSell(!Privacy.HAS_ATT_CONSENT);
            MaxSdk.SetIsAgeRestrictedUser(false);

            // FB
#if UNITY_IOS
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(Privacy.HAS_ATT_CONSENT);
#endif
            if (Privacy.HAS_ATT_CONSENT) {
                AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });
            } else {
                AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { "LDU" }, 0, 0);
            }

            // Privacy string
            char[] privacyCharacters = new char[4];
            privacyCharacters[0] = '1';
            privacyCharacters[1] = 'Y';
            if (Privacy.HAS_ATT_CONSENT) {
                privacyCharacters[2] = 'N';
            } else {
                privacyCharacters[2] = 'Y';
            }
            privacyCharacters[3] = 'N';

            Privacy.CCPA_STRING = new string(privacyCharacters);

            EverydayLogger.Debug($"Is GDPR applicable?: {Privacy.IS_GDPR_APPLICABLE}");
            EverydayLogger.Debug($"GDPR string: {Privacy.TCString}");
            EverydayLogger.Debug($"Privacy string: {Privacy.CCPA_STRING}");
        }
    }
}
#endif
