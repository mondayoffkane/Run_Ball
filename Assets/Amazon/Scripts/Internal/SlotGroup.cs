using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AmazonAds{
public class SlotGroup
{
    ISlotGroup client;

    public SlotGroup(string slotGroupName){
        #if UNITY_ANDROID
            client = new AmazonAds.Android.DTBSlotGroup(slotGroupName);
        #elif UNITY_IOS
            client = new AmazonAds.IOS.DTBSlotGroup(slotGroupName);
        #endif
    }

    public void AddSlot(int width, int height, string uid){
        AdSize size = new AdSize(width, height, uid);
        AddSlot(size);
    }

    public void AddSlot(AdSize size){
        client.AddSlot(size);
    }

    public ISlotGroup GetInstance(){
        return client;
    }
}
}

