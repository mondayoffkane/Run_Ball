using UnityEngine;
using UnityEditor;
using Facebook.Unity.Settings;
using Facebook.Unity.Editor;
using System.Collections.Generic;
using System.Threading;

namespace MondayOFF
{
    public class EverydaySettingsWindow : EditorWindow
    {
        const float IMAGE_HEIGHT = 100f;
        const float PADDING = 10f;
        // const float SMALL_BUTTON_WIDTH = 20f;
        const float LABEL_WIDTH = 250f;
        const float MENU_GAP = 10f;
        const float ITEM_GAP = 5f;
        const float TINTY_GAP_FOR_OVERSIZED_TEXTURE = 25f;
        // const float MESSAGE_BOX_HEIGHT = 55f;
        readonly static Vector2 WINDOW_SIZE = new Vector2(650f, 650f);

        // internal string message = default;
        // internal bool isWorking = false;
        // internal CancellationTokenSource cancellationTokenSource = default;

        Vector2 _scrollPosition = Vector2.zero;
        GUIStyle _largeAndBoldLabel = null;
        GUIStyle _textAreaStyle = null;
        Texture2D _logo = null;
        bool _expandAppSettings = true;
        bool _expandGeneralSettings = true;
        bool _expandAdSettings = true;

        public static void Open()
        {
            if (EverydaySettings.Instance == null)
            {
                EverydayLogger.Error("EverydaySettings is null!");
                return;
            }
            // var window = EditorWindow.GetWindowWithRect<EverydaySettingsWindow>(new Rect(100, 100, position.width, position.height), true, "Every Settings Window", true);
            var window = EditorWindow.GetWindow<EverydaySettingsWindow>(true, "Every Settings Window", true);
            window.Show();
        }

        private void OnGUI()
        {
            if (_largeAndBoldLabel == null)
            {
                _largeAndBoldLabel = new GUIStyle(EditorStyles.largeLabel)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    // _largeAndBoldLabel.fixedHeight = EditorGUIUtility.singleLineHeight * 1.5f;
                    border = new RectOffset(0, 0, 0, 0)
                };
            }

            if (_textAreaStyle == null)
            {
                _textAreaStyle = new GUIStyle(EditorStyles.textArea)
                {
                    wordWrap = true
                };
            }

            if (_logo == null)
            {
                _logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/MondayOFF/Everyday/Textures/Editor/mondayoff_logo_transparency.png");
            }

            // make this window non-resizable
            minSize = WINDOW_SIZE;

            var settings = EverydaySettings.Instance;

            // Show logo
            GUI.DrawTexture(new Rect(0f, -15f, position.width, IMAGE_HEIGHT), _logo, ScaleMode.ScaleToFit, true, 0f);
            EditorGUILayout.Space(IMAGE_HEIGHT - TINTY_GAP_FOR_OVERSIZED_TEXTURE - 20f);
            GUILayout.Label($"v{MondayOFF.EveryDay.Version}", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Italic, fontSize = 12 });
            // if (!string.IsNullOrEmpty(message)) {
            //     // Show message box
            //     GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            //     style.alignment = TextAnchor.MiddleCenter;
            //     GUI.Box(new Rect(10, 10 + IMAGE_HEIGHT - TINTY_GAP_FOR_OVERSIZED_TEXTURE, position.width - 20, MESSAGE_BOX_HEIGHT - 20), message, style);
            //     GUILayout.BeginArea(
            //         new Rect(
            //             PADDING,
            //             PADDING + IMAGE_HEIGHT - TINTY_GAP_FOR_OVERSIZED_TEXTURE + MESSAGE_BOX_HEIGHT,
            //         position.width - PADDING * 2,
            //         position.height - PADDING * 2 - IMAGE_HEIGHT + TINTY_GAP_FOR_OVERSIZED_TEXTURE - MESSAGE_BOX_HEIGHT
            //         )
            //     );
            // } else {
            GUILayout.BeginArea(
                new Rect(
                    PADDING,
                    PADDING + IMAGE_HEIGHT - TINTY_GAP_FOR_OVERSIZED_TEXTURE,
                    position.width - PADDING * 2,
                    position.height - PADDING * 2 - IMAGE_HEIGHT + TINTY_GAP_FOR_OVERSIZED_TEXTURE - 50f
                ), GUI.skin.box
            );
            // }

            // if (isWorking) {
            //     EditorGUI.BeginDisabledGroup(true);
            // }

            // begin vertical scroll
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
            EditorGUILayout.BeginVertical();

            AddDivider();
            ExpandableLabel("General", ref _expandGeneralSettings);
            if (_expandGeneralSettings)
            {
                EditorGUI.indentLevel++;

                AddField(
                    "Initialize on Launch [?]",
                    "Initialize Everyday and related third parties on launch",
                    () => EditorGUILayout.Toggle(settings.initializeOnLaunch),
                    (value) => { settings.initializeOnLaunch = (bool)value; },
                    settings
                );

                if (settings.initializeOnLaunch)
                {
                    EditorGUI.indentLevel++;
                    AddField(
                        "Initialization Delay [?]",
                        "Default is 2 seconds. Goal is to display ATT dialog after splash image",
                        () => EditorGUILayout.Slider(settings.initializationDelay, 0f, 5f),
                        (value) => { settings.initializationDelay = value; },
                        settings
                    );
                    EditorGUI.indentLevel--;
                }

                // Show LogLevel
                AddField(
                    "Log Level [?]",
                    "Filter log messages by log level.\nNOTE: Editor will always log everything.",
                    () => (LogLevel)EditorGUILayout.EnumPopup(settings.logLevel),
                    (value) => { settings.logLevel = (LogLevel)value; },
                    settings
                );

                // Set Test Mode
                AddField(
                    "Test Mode [?]",
                    "Enable test mode to test your app without submitting it for review.",
                    () => EditorGUILayout.Toggle(settings.isTestMode),
                    (value) => { settings.isTestMode = (bool)value; },
                    settings
                );

                EditorGUILayout.Space(MENU_GAP);
                EditorGUI.indentLevel--;
            }

            AddDivider();
            ExpandableLabel("App Settings", ref _expandAppSettings);
            if (_expandAppSettings)
            {
                // EditorGUILayout.BeginHorizontal();
                // GUILayout.FlexibleSpace();
                // if (GUILayout.Button("Get App Settings", GUILayout.Height(EditorGUIUtility.singleLineHeight * 1.5f))) {
                //     if (cancellationTokenSource != null) {
                //         cancellationTokenSource.Cancel();
                //         cancellationTokenSource.Dispose();
                //     }
                //     cancellationTokenSource = new CancellationTokenSource();
                //     ProjectSettingsDownloader.FetchAppSettings(this);
                //     return;
                // }
                // GUILayout.FlexibleSpace();
                // EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Identification", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // app guid
                // EditorGUILayout.BeginHorizontal();
                // EditorGUILayout.LabelField(new GUIContent("Game Id [?]", "GUID of the app"), GUILayout.Width(LABEL_WIDTH));
                // var gameId = EditorGUILayout.TextField(settings.gameId);
                // if (GUILayout.Button("Get Game Settings")) {
                //     if (cancellationTokenSource != null) {
                //         cancellationTokenSource.Cancel();
                //         cancellationTokenSource.Dispose();
                //     }
                //     cancellationTokenSource = new CancellationTokenSource();
                //     ProjectSettingsDownloader.FetchAppSettings(this);
                //     return;
                // }
                // EditorGUILayout.EndHorizontal();
                // if (string.IsNullOrEmpty(gameId)) {
                //     EditorGUILayout.HelpBox("Please enter Game Id(GUID)", MessageType.Info);
                // }

                // bundle ID
                AddField(
                    "Bundle ID [?]",
                    "Bundle ID/Package Name of the game",
                    () =>
                    {
                        return EditorGUILayout.TextField(PlayerSettings.applicationIdentifier);
                    },
                    (value) =>
                    {
                        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, (string)value);
                        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, (string)value);
                        PlayerSettings.applicationIdentifier = value;
                    },
                    null
                );

                // Platform
                AddReadOnlyField(
                    "Platform [?]",
                    "Current build target platform",
                    () =>
                    {
                        EditorGUILayout.TextField(EditorUserBuildSettings.activeBuildTarget.ToString());
                    }
                );

                EditorGUILayout.Space(MENU_GAP);

                EditorGUI.indentLevel--;
                // label with bold font
                EditorGUILayout.LabelField("Facebook", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // Facebook App ID
                AddField(
                    "Facebook App ID",
                    null,
                    () =>
                    {
                        return EditorGUILayout.TextField(FacebookSettings.AppId);
                    },
                    (value) =>
                    {
                        FacebookSettings.AppIds = new List<string>() { ((string)value).Trim() };
                    },
                    FacebookSettings.Instance
                );
                if (string.IsNullOrEmpty(FacebookSettings.AppId) || FacebookSettings.AppId == "0")
                {
                    EditorGUILayout.HelpBox("Facebook App ID is empty!", MessageType.Error);
                }

                // Facebook Client Token
                AddField(
                    "Facebook Client Token",
                    null,
                    () =>
                    {
                        return EditorGUILayout.TextField(FacebookSettings.ClientToken);
                    }
                    ,
                    (value) =>
                    {
                        FacebookSettings.ClientTokens = new List<string>() { ((string)value).Trim() };
                    },
                    FacebookSettings.Instance
                );
                if (string.IsNullOrEmpty(FacebookSettings.ClientToken))
                {
                    EditorGUILayout.HelpBox("Facebook Client Token is empty!", MessageType.Error);
                }

                // button to regenerate AndroidManifest.xml using Facebook settings
                // GUILayout.BeginHorizontal();
                // GUILayout.Label("", GUILayout.Width(LABEL_WIDTH));
                // if (GUILayout.Button("Regenerate Android Manifest")) {
                //     ManifestMod.GenerateManifest();
                // }
                // GUILayout.EndHorizontal();

                EditorGUILayout.Space(MENU_GAP);
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("AdMob IDs", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // AdMob App ID
                AddField(
                    "Android",
                    null,
                    () =>
                    {
                        return EditorGUILayout.TextField(AppLovinSettings.Instance.AdMobAndroidAppId);
                    },
                    (value) =>
                    {
                        AppLovinSettings.Instance.AdMobAndroidAppId = ((string)value).Trim();
                    },
                    AppLovinSettings.Instance
                );

                // AdMob App ID iOS
                AddField(
                    "iOS",
                    null,
                    () =>
                    {
                        return EditorGUILayout.TextField(AppLovinSettings.Instance.AdMobIosAppId);
                    },
                    (value) =>
                    {
                        AppLovinSettings.Instance.AdMobIosAppId = ((string)value).Trim();
                    },
                    AppLovinSettings.Instance
                );

                EditorGUILayout.Space(MENU_GAP);

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            // Show AdSettings
            AddDivider();
            ExpandableLabel("Ad Settings", ref _expandAdSettings);
            if (_expandAdSettings && settings.adSettings != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Reset to default", GUILayout.Width(150f)))
                {
                    settings.adSettings = new AdSettings();
                    settings.adSettings.playOnPosition = new PlayOnPosition();
                    EditorUtility.SetDirty(settings);
                    this.Repaint();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Ad Unit IDs", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Android", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // Android Ad Unit ID Interstitial
                AddField(
                    "Interstitial",
                    "Ad Unit ID for Interstitial",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_IS_AdUnitID),
                    (value) => { settings.adSettings.AOS_IS_AdUnitID = ((string)value).Trim(); },
                    settings
                );
                // Android Ad Unit ID Rewarded
                AddField(
                    "Rewarded",
                    "Ad Unit ID for Rewarded",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_RV_AdUnitID),
                    (value) => { settings.adSettings.AOS_RV_AdUnitID = ((string)value).Trim(); },
                    settings
                );
                // Android Ad Unit ID Banner
                AddField(
                    "Banner",
                    "Ad Unit ID for Banner",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_BN_AdUnitID),
                    (value) => { settings.adSettings.AOS_BN_AdUnitID = ((string)value).Trim(); },
                    settings
                );

                // BACKUP
                // EditorGUI.indentLevel++;
                // // Android BACKUP Ad Unit ID Interstitial
                // AddField(
                //     "Backup Interstitial",
                //     "Backup Ad Unit ID for Interstitial",
                //     () => EditorGUILayout.TextField(settings.adSettings.AOS_IS_Backup_AdUnitID),
                //     (value) => { settings.adSettings.AOS_IS_Backup_AdUnitID = ((string)value).Trim(); },
                //     settings
                // );
                // // Android BACKUP Ad Unit ID Rewarded
                // AddField(
                //     "Backup Rewarded",
                //     "Backup Ad Unit ID for Rewarded",
                //     () => EditorGUILayout.TextField(settings.adSettings.AOS_RV_Backup_AdUnitID),
                //     (value) => { settings.adSettings.AOS_RV_Backup_AdUnitID = ((string)value).Trim(); },
                //     settings
                // );
                // // Android BACKUP Ad Unit ID Banner
                // AddField(
                //     "Backup Banner",
                //     "Backup Ad Unit ID for Banner",
                //     () => EditorGUILayout.TextField(settings.adSettings.AOS_BN_Backup_AdUnitID),
                //     (value) => { settings.adSettings.AOS_BN_Backup_AdUnitID = ((string)value).Trim(); },
                //     settings
                // );
                // EditorGUI.indentLevel--;

                EditorGUILayout.Space(ITEM_GAP);
                // Android APS App ID
                AddField(
                    "APS App ID",
                    "App ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_APS_AppID),
                    (value) => { settings.adSettings.AOS_APS_AppID = ((string)value).Trim(); },
                    settings
                );
                // Android APS Interstitial ID
                AddField(
                    "APS Interstitial ID",
                    "Interstitial ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_APS_IS_SlotID),
                    (value) => { settings.adSettings.AOS_APS_IS_SlotID = ((string)value).Trim(); },
                    settings
                );
                // Android APS Rewarded ID
                AddField(
                    "APS Rewarded ID",
                    "Rewarded ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_APS_RV_SlotID),
                    (value) => { settings.adSettings.AOS_APS_RV_SlotID = ((string)value).Trim(); },
                    settings
                );
                // Android APS Banner ID
                AddField(
                    "APS Banner ID",
                    "Banner ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_APS_BN_SlotID),
                    (value) => { settings.adSettings.AOS_APS_BN_SlotID = ((string)value).Trim(); },
                    settings
                );
                EditorGUILayout.Space(ITEM_GAP);
                // Android PlayOn Key
                AddField(
                    "PlayOn Key",
                    "Key for PlayOn",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_PlayOnApiKey),
                    (value) => { settings.adSettings.AOS_PlayOnApiKey = ((string)value).Trim(); },
                    settings
                );
                EditorGUILayout.Space(ITEM_GAP);
                // Android Adverty Key
                AddField(
                    "Adverty Key",
                    "Key for Adverty",
                    () => EditorGUILayout.TextField(settings.adSettings.AOS_AdvertyApiKey),
                    (value) => { settings.adSettings.AOS_AdvertyApiKey = ((string)value).Trim(); },
                    settings
                );


                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("iOS", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // iOS Ad Unit ID Interstitial
                AddField(
                    "Interstitial",
                    "Ad Unit ID for Interstitial",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_IS_AdUnitID),
                    (value) => { settings.adSettings.iOS_IS_AdUnitID = ((string)value).Trim(); },
                    settings
                );
                // iOS Ad Unit ID Rewarded
                AddField(
                    "Rewarded",
                    "Ad Unit ID for Rewarded",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_RV_AdUnitID),
                    (value) => { settings.adSettings.iOS_RV_AdUnitID = ((string)value).Trim(); },
                    settings
                );
                // iOS Ad Unit ID Banner
                AddField(
                    "Banner",
                    "Ad Unit ID for Banner",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_BN_AdUnitID),
                    (value) => { settings.adSettings.iOS_BN_AdUnitID = ((string)value).Trim(); },
                    settings
                );

                // BACKUP
                // EditorGUI.indentLevel++;
                // // iOS BACKUP Ad Unit ID Interstitial
                // AddField(
                //     "Backup Interstitial",
                //     "Backup Ad Unit ID for Interstitial",
                //     () => EditorGUILayout.TextField(settings.adSettings.iOS_IS_Backup_AdUnitID),
                //     (value) => { settings.adSettings.iOS_IS_Backup_AdUnitID = ((string)value).Trim(); },
                //     settings
                // );
                // // iOS BACKUP Ad Unit ID Rewarded
                // AddField(
                //     "Backup Rewarded",
                //     "Backup Ad Unit ID for Rewarded",
                //     () => EditorGUILayout.TextField(settings.adSettings.iOS_RV_Backup_AdUnitID),
                //     (value) => { settings.adSettings.iOS_RV_Backup_AdUnitID = ((string)value).Trim(); },
                //     settings
                // );
                // // iOS BACKUP Ad Unit ID Banner
                // AddField(
                //     "Backup Banner",
                //     "Backup Ad Unit ID for Banner",
                //     () => EditorGUILayout.TextField(settings.adSettings.iOS_BN_Backup_AdUnitID),
                //     (value) => { settings.adSettings.iOS_BN_Backup_AdUnitID = ((string)value).Trim(); },
                //     settings
                // );
                // EditorGUI.indentLevel--;

                EditorGUILayout.Space(ITEM_GAP);
                // iOS APS App ID
                AddField(
                    "APS App ID",
                    "App ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_APS_AppID),
                    (value) => { settings.adSettings.iOS_APS_AppID = ((string)value).Trim(); },
                    settings
                );
                // iOS APS Interstitial ID
                AddField(
                    "APS Interstitial ID",
                    "Interstitial ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_APS_IS_SlotID),
                    (value) => { settings.adSettings.iOS_APS_IS_SlotID = ((string)value).Trim(); },
                    settings
                );
                // iOS APS Rewarded ID
                AddField(
                    "APS Rewarded ID",
                    "Rewarded ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_APS_RV_SlotID),
                    (value) => { settings.adSettings.iOS_APS_RV_SlotID = ((string)value).Trim(); },
                    settings
                );
                // iOS APS Banner ID
                AddField(
                    "APS Banner ID",
                    "Banner ID for APS",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_APS_BN_SlotID),
                    (value) => { settings.adSettings.iOS_APS_BN_SlotID = ((string)value).Trim(); },
                    settings
                );
                EditorGUILayout.Space(ITEM_GAP);
                // iOS PlayOn Key
                AddField(
                    "PlayOn Key",
                    "Key for PlayOn",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_PlayOnApiKey),
                    (value) => { settings.adSettings.iOS_PlayOnApiKey = ((string)value).Trim(); },
                    settings
                );
                // iOS PlayOn Store ID
                AddField(
                    "PlayOn Store ID",
                    "Store ID for PlayOn",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_storeID),
                    (value) => { settings.adSettings.iOS_storeID = ((string)value).Trim(); },
                    settings
                );
                EditorGUILayout.Space(ITEM_GAP);
                // iOS Adverty Key
                AddField(
                    "Adverty Key",
                    "Key for Adverty",
                    () => EditorGUILayout.TextField(settings.adSettings.iOS_AdvertyApiKey),
                    (value) => { settings.adSettings.iOS_AdvertyApiKey = ((string)value).Trim(); },
                    settings
                );


                EditorGUILayout.Space(ITEM_GAP);

                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Initialization", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // Initialize Ads on load
                AddField(
                    "Initialize AdsManager on load [?]",
                    "Enable it to initialize AdsManager upon launch",
                    () => EditorGUILayout.Toggle(settings.adSettings.initializeOnLoad),
                    (value) => { settings.adSettings.initializeOnLoad = (bool)value; },
                    settings
                );

                // Ad initialization order
                AddField(
                    "Ad initialization order [?]",
                    "It is recommended to separate resource heavy ad loadings.\nSelect loading order that suits your game",
                    () => EditorGUILayout.EnumPopup(settings.adSettings.adInitializationOrder),
                    (value) => { settings.adSettings.adInitializationOrder = (AdInitializationOrder)value; },
                    settings
                );

                // Delay between ad initialization with range 0-10
                AddField(
                    "Delay between ad initialization [?]",
                    "Delay between each ad type load (in seconds)",
                    () => EditorGUILayout.Slider(settings.adSettings.adInitializationDelay, 0f, 3f),
                    (value) =>
                    {
                        settings.adSettings.adInitializationDelay = (float)value;
                    },
                    settings
                );

                EditorGUILayout.Space(ITEM_GAP);

                // Interstitial
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Interstitial", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                AddField(
                    "Interstitial Interval [?]",
                    "Minimum interval between interstitial ads (in seconds)",
                    () => EditorGUILayout.IntSlider((int)settings.adSettings.interstitialInterval, 0, 60),
                    (value) => { settings.adSettings.interstitialInterval = (int)value; },
                    settings
                );

                // Rewarded
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Rewarded", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Reset interstitial interval after rewarded
                AddField(
                    "Also reset interstitial interval [?]",
                    "Resets interstitial interval timer when rewarded ad is closed",
                    () => EditorGUILayout.Toggle(settings.adSettings.resetTimerOnRewarded),
                    (value) => { settings.adSettings.resetTimerOnRewarded = (bool)value; },
                    settings
                );

                // Banner
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Banner", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Show Banner on initialization
                AddField(
                    "Show Banner on load [?]",
                    "Enable it to show banner when AdsManager is initialized",
                    () => EditorGUILayout.Toggle(settings.adSettings.showBannerOnLoad),
                    (value) => { settings.adSettings.showBannerOnLoad = (bool)value; },
                    settings
                );

                // banner position
                AddField(
                    "Banner position [?]",
                    "Select banner position",
                    () => EditorGUILayout.EnumPopup(settings.adSettings.bannerPosition),
                    (value) => { settings.adSettings.bannerPosition = (MaxSdkBase.BannerPosition)value; },
                    settings
                );

                EditorGUILayout.Space(ITEM_GAP);

                // Play On Positions
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("PlayOn", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                AddField(
                    "Show PlayOn after Interstitials [?]",
                    "Automatically display PlayOn Icon ads after watching set number of interstitials.",
                    () => EditorGUILayout.Toggle(settings.adSettings.showPlayOnAfterInterstitial),
                    (value) => { settings.adSettings.showPlayOnAfterInterstitial = value; },
                    settings
                );

                if (settings.adSettings.showPlayOnAfterInterstitial)
                {
                    AddField(
                        "     Every # interstitials",
                        null,
                        () => EditorGUILayout.IntSlider(settings.adSettings.playOnInterstitialCount, 1, 4),
                        (value) => { settings.adSettings.playOnInterstitialCount = (int)value; },
                        settings
                    );
                }

                AddField(
                    "Use Screen Positioning [?]",
                    "Absolute screen position.\nNote: Logo may appear slightly different on actual devices.",
                    () => EditorGUILayout.Toggle(settings.adSettings.playOnPosition.useScreenPositioning),
                    (value) => { settings.adSettings.playOnPosition.useScreenPositioning = (bool)value; },
                    settings
                );

                if (settings.adSettings.playOnPosition.useScreenPositioning)
                {
                    // not using indent because it tilts input field as well
                    // PlayOn logo anchor
                    AddField(
                        "     Logo Anchor [?]",
                        "Select logo anchor",
                        () => EditorGUILayout.EnumPopup(settings.adSettings.playOnPosition.playOnLogoAnchor),
                        (value) => { settings.adSettings.playOnPosition.playOnLogoAnchor = (PlayOnSDK.Position)value; },
                        settings
                    );

                    // PlayOn Logo Offset
                    AddField(
                        "     Logo Offset [?]",
                        "Logo offset from selected anchor",
                        () => EditorGUILayout.Vector2IntField("", settings.adSettings.playOnPosition.playOnLogoOffset),
                        (value) => { settings.adSettings.playOnPosition.playOnLogoOffset = (Vector2Int)value; },
                        settings
                    );

                    // PlayOn Logo Size
                    AddField(
                        "     Logo Size [?]",
                        "Logo size",
                        () => EditorGUILayout.IntField("", settings.adSettings.playOnPosition.playOnLogoSize),
                        (value) => { settings.adSettings.playOnPosition.playOnLogoSize = (int)value; },
                        settings
                    );
                }

                EditorGUILayout.Space(ITEM_GAP);
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Adverty", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Adverty Initialize on awake
                AddField(
                    "Adverty Initialize on awake [?]",
                    "Enable it to initialize Adverty SDK on awake",
                    () => EditorGUILayout.Toggle(settings.adSettings.initializeAdvertyOnAwake),
                    (value) => { settings.adSettings.initializeAdvertyOnAwake = (bool)value; },
                    settings
                );

                EditorGUILayout.Space(MENU_GAP);
                EditorGUI.indentLevel--;
            }

            AddDivider();

            EditorGUILayout.Space(100f);

            EditorGUILayout.EndVertical();

            // end vertical scroll
            EditorGUILayout.EndScrollView();

            // if (isWorking) {
            //     EditorGUI.EndDisabledGroup();
            // }


            GUILayout.EndArea();

            // Guilayout anchord bottom
            GUILayout.BeginArea(new Rect(10f, position.height - 55f, position.width - 20f, 45f));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", GUILayout.Height(50f), GUILayout.Width(LABEL_WIDTH)))
            {
                Close();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void AddDivider()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void ExpandableLabel(in string label, ref bool isExpanded)
        {
            if (GUILayout.Button(string.Format("{0} {1}", isExpanded ? "▼" : "▶", label), _largeAndBoldLabel))
            {
                isExpanded = !isExpanded;
            }
        }

        private void AddField<T>(in string labelText, in string tooltip, in System.Func<T> getter, in System.Action<T> setter, in Object targetObject)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(new GUIContent(labelText, tooltip), GUILayout.Width(LABEL_WIDTH));
            var value = getter();
            if (EditorGUI.EndChangeCheck())
            {
                if (targetObject != null)
                {
                    Undo.RecordObject(targetObject, labelText);
                }
                setter(value);
                if (targetObject != null)
                {
                    EditorUtility.SetDirty(targetObject);
                    AssetDatabase.SaveAssetIfDirty(targetObject);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void AddReadOnlyField(in string labelText, in string tooltip, in System.Action display)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(labelText, tooltip), GUILayout.Width(LABEL_WIDTH));
            EditorGUI.BeginDisabledGroup(true);
            display();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        // private void OnDestroy() {
        //     cancellationTokenSource?.Cancel();
        //     cancellationTokenSource?.Dispose();
        // }
    }
}