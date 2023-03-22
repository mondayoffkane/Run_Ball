namespace MondayOFF {
    [System.Flags]
    public enum AdType : byte {
        Banner = 0b001,
        Interstitial = 0b010,
        Rewarded = 0b100,
        All = 0b111,
    }
}