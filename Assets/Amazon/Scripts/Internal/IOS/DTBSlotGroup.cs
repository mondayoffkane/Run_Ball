using System;

namespace AmazonAds.IOS
{
    public class DTBSlotGroup : ISlotGroup
    {
        IntPtr client;
        public DTBSlotGroup(string slotGroupName)
        {
            client = Externs._createSlotGroup(slotGroupName);
        }

        public void AddSlot(int width, int height, string uid)
        {
            AdSize size = new AdSize(width, height, uid);
            AddSlot(size);
        }

        public void AddSlot(AdSize size)
        {
            DTBAdSize newSize = (DTBAdSize)size.GetInstance();
            Externs._addSlot(client, newSize.GetInstance());
        }

        public IntPtr GetInstance()
        {
            return client;
        }
    }
}

