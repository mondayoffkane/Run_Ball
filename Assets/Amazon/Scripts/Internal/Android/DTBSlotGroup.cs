namespace AmazonAds.Android
{
    public class DTBSlotGroup : ISlotGroup
    {
        Android.AdRegistration.SlotGroup client;
        public DTBSlotGroup()
        {
            client = new Android.AdRegistration.SlotGroup();
        }

        public DTBSlotGroup(string slotGroupName)
        {
            client = new Android.AdRegistration.SlotGroup(slotGroupName);
        }

        public void AddSlot(int width, int height, string uid)
        {
            AdSize size = new AdSize(width, height, uid);
            AddSlot(size);
        }

        public void AddSlot(AdSize size)
        {
            client.AddSlot(size.GetInstance());
        }

        public Android.AdRegistration.SlotGroup GetInstance()
        {
            return client;
        }
    }
}

