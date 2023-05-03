namespace AmazonAds{
    public interface ISlotGroup
    {
        void AddSlot(int width, int height, string uid);
        void AddSlot(AdSize size);
    }
}

