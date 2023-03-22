namespace MondayOFF {
    [System.Serializable]
    internal enum AdInitializationOrder : ushort {
        // 0b_001 = banner
        // 0b_010 = interstitial
        // 0b_100 = rewarded

        // 0b_010_100_001
        Inter_Reward_Banner = AdType.Interstitial << 6 | AdType.Rewarded << 3 | AdType.Banner << 0,
        Inter_Banner_Reward = AdType.Interstitial << 6 | AdType.Banner << 3 | AdType.Rewarded << 0,

        Reward_Inter_Banner = AdType.Rewarded << 6 | AdType.Interstitial << 3 | AdType.Banner << 0,
        Reward_Banner_Inter = AdType.Rewarded << 6 | AdType.Banner << 3 | AdType.Interstitial << 0,

        Banner_Inter_Reward = AdType.Banner << 6 | AdType.Interstitial << 3 | AdType.Rewarded << 0,
        Banner_Reward_Inter = AdType.Banner << 6 | AdType.Rewarded << 3 | AdType.Interstitial << 0,
    }
}