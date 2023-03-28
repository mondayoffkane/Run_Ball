# Everyday Unity Plugin

This package consists of these third party SDKs
- Singular SDK 
- Facebook SDK
- AppLovin MAX SDK
- PlayOn SDK
- Adverty SDK
- Firebase Analytics
---

## Migrating to v3.0.0
- **Don't**. 
-  It has no-backward compatibility so clean up Everyday before integrating version 3 if you have older version installed.

## Requirements
- Unity 2021.3+

## Installation
1. Import Everyday Unity package to the project
1. Go to Meun -> Facebook -> Edit Settings
    - Set Facebook **App name**, **Facebook App ID**, and **Client Token**
    - Click `Regenerate Android Manifest` when everything is filled in
        ![FacebookSettings](./res/FacebookSettings.png)
    > ## Client Token
    > #### As of Facebook SDK 14.0, **Client Token** is no longer optional. 
    > #### You can find it in your Facebook App > Settings > Advanced
        > ![FacebookClientToken](./res/FacebookClientToken.png)
1. Go to Menu -> Everyday ->  Open Everyday Settings
    - **Enable Verbose Logging**
        - Enable it to use default Unity Debug logger. Disabling it will hide **ALL** Unity log messages. It is recommended to disable verbose logging on release products.
    - **Initialize On Load**
        - Enable it to initialze AdsManager upon initialization. 
        > Call `MondayOFF.AdsManager.Initialize()` if you want to manually initialize AdsManager.
    - **Show Banner On Load**
        - Enable it to show banner when AdsManager is initialized.
    - **Ad Initialization Order**
        - It is recommended to separate resource heavy ad loadings. Select loading order that suits your game.
    - **Delay**
        - Delay between each loading (in seconds).
    - **Interstitial Interval**
        - A minimum time span between interstitial Ads (in seconds). AdsManager will display consequent interstitial *ONLY AFTER* set interval has passed since the last interstitial ad.
    - **Reset Timer On Rewarded**
        - Resets interval timer after rewarded ad (Default: false)
    - **Banner Position**
        - Set banner position. Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
    - **Ad Unit ID**
        - Ad Unit IDs for each platform and ad format. Empty Ad Unit IDs will be considered inactive.
    - **PlayOn Api Key**
        - Enter Api Keys for Android and iOS
    - **AppStore ID**
        - (iOS only) Enter store ID (ex/ 1485533179)
    - **Play PlayOn Every Nth Interstitial**
        - Display PlayOn after every nth interstitial. If nth interstitial has been reached but PlayOn is not ready, it will try to show on the next interstitial. Set it < 0 if you want to show PlayOn manually.
        
    #### **Positioning**
    > - **Use Screen Position**
    >    - Set position of PlayOn ad
    > - **Logo Anchor Position**
    >    - Anchor position of logo ad 
    >- **Logo Offset**
    >    - Logo position offset from anchor
    >- **Logo Size**
    >    - Size of logo ad. Size may look different on actual device so please test it.
    - **Initialize Adverty On Awake**
        - Initialize Adverty at startup and sets `Camera.main` as main camera.
    - **Adverty Api Key**
        - Enter Api Keys for Android and iOS
         

# Usage
## Initialization
- Plugin is automatically initialized when the game starts
## Ads
### **Initialization**
- Initialize `AdsManager` manually only when `Initialize On Load` is not enabled.
```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    private void Start() {
        // If you have No Ads In-app purchase, check before initialization.
        if (SaveSystem.GetValue("NoAds") == true) {
            // If you choose to show banner even after user purchases No Ads product, put banner only.
            AdsManager.Initialize(AdType.Banner);
        } else {
            // You can specify which ad types to use in the parameter. AdType is enum.
            // Default is AdType.All (equivalent to AdType.Banner | AdType.Interstitial | AdType.Rewarded)
            AdsManager.Initialize(AdType.Banner | AdType.Interstitial);
        }
    }
    // ...
}
```
### **Interstitial**
```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void OnStageComplete(){
        // Returns true and shows ad if specified interval has passed since the last display
        // (ex/ If a user sees interstitial ad at the end of a stage and clears another stage in 15 seconds, next `ShowInterstitial()` will not display interstitial ad and return false)
        bool isDisplayed = AdsManager.ShowInterstitial();

    }
    // ...
}
```
#### **Rewarded**
> Don't forget to set rewarding method as parameter

```C#
using UnityEngine;
using UnityEngine.UI;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    // Button to show rewarded ad and grant reward
    public Button rewardedAdButton;

    private void Start(){
        // Delegate is called when rewarded Ad is loaded.
        // You can add delegate to enable button when rewarded is loaded
        AdsManager.OnRewardedAdLoaded += () => rewardedAdButtn.interactable = true;

        // Disable button interaction if rewarded is not loaded
        rewardedAdButton.interactable = false;
        
        rewardedAdButton.onClick.AddListener(OnRewardedButtonClicked);
    }

    private void Update(){
        // Constantly check load status if you choose not to use OnRewardedAdLoaded delegate
        if (AdsManager.IsRewardedReady() == true) {
            rewardedAdButton.interactable = true;
        }
    }
    
    public void OnRewardedButtonClicked(){
        // Make sure to grant reward after watching rewarded ad
        AdsManager.ShowRewarded(() => Debug.Log("Player receives reward"));
    }
    // ...
}
```
#### **Banners**
> If **Show Banner On Load** is enabled, banner will be displayed when game starts
```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void OnStageStart(){
        AdsManager.instance.ShowBanner();
    }

    public void OnStageEnd(){
        AdsManager.instance.HideBanner();
    }
    // ...
}
```
#### **Disabling Ad types**
- To disable some ad types, upon purchasing No Ads for instance, use methods below.
> BN is banner, IS is interstitial, RV is rewarded
```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void OnPurchaseNoAds() {
        // Disable specific type of ad
        AdsManager.DisableIS();
        AdsManager.DisableBN();
        AdsManager.DisableRV();

        // You can also disable multiple ads using AdType enum
        AdsManager.DisableAdType(AdType.Interstitial | AdType.Banner);
    }
    // ...
}
```
#### Ad Related Callbacks
- Just like `AdsManager.OnRewardedAdLoaded()`, MaxSdk supports many more callbacks related to ad status.
- Use `MaxSdkCallbacks.Banner`, `MaxSdkCallbacks.Interstitial`, and `MaxSdkCallbacks.Rewarded` to directly access MaxSdk callbacks.

## PlayOn
> See [Integration Guide Document](./res/PlayOn%20Integration%20Guide.pdf) for more details.
- As of PlayOn 2.0.9, they support positioning logo ad based on RectTransform. You can add component `PlayOnAnchor` to an RectTransform to locate logo ad inside it. Keep in mind that it is NOT displayed inside RectTransform like Unity UI component. It CAPTURES screen points relative to RectTransfom and displays it on native layer.
- You can also call `AdsManager.LinkLogoToRectTransform()` directly.
- Logo ad size is automatically calculated based on size of RectTransform. It will not display anything if RectTransform is too small. Logo size can not be larger than allowed size regardless of RectTransform size.
- (NOT RECOMMENDED) If you really need to use old positioning, where you had to put arbitary coordinates which was not adaptive to screen resolution, you can do it by check `Use Legacy Play On Positioning`.

## Adverty
> See [Documentation](https://adverty.com/4.1/documentation) for more details
- Check `initializeAdvertyOnAwake` if your game have main camera at the start of game scene and is the only camera throughout the gameplay.
- Call `InitializeAdverty(Camera)` if you want to manually initialze Adverty.
- Call `ChangeAdvertyCamera(Camera)` if your main camera changes during gameplay.

## Events
### **Initialization**
- Event tracker is initialized automatically.
### **Stage Try/Clear**
```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void OnStageStart(int stageNum) {
        // Send try event at the beginning of stage
        EventTracker.TryStage(stageNum);
    }

    public void OnStageClear(int stageNum) {
        // Send try event at the beginning of stage
        EventTracker.TryStage(stageNum);
    }
    // ...
}
```
### **Custom Events**
```C#
using System.Collections.Generic;
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void OnStageFail(int stageNum) {
        // Do NOT send clear event when stage fails.
        // Send custom event if you want to track stage fail event
        var params = new Dictionary<string, string>{
            {"Stage", stageNum.ToString()}
        };
        EventTracker.LogCustomEvent("StageFailed", params);
    }
    // ...
}
```
## In-App Purchase
### **Initialization**
- IAP Manager is initialized automatically.
### **Registration and purchase of item**
```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void RegisterItem0() {
        // Register reward BEFORE purchasing item. 
       var status = IAPManager.RegisterProduct("Item0", RewardForItem0);
    }

    public void PurchaseItem0() {
        // PurchaseProduct will return false if reward is NOT registered
        var status = IAPManager.PurchaseProduct("Item0");
        switch(status) {
            case IAPStatus.ProductNotRegistered:
            // Register and try purchasing again if possible
            break;
       }
    }

    public void OneShotPurchase0() {
        // Warning! It will overwrite already registered callbacks
        var status = IAPManager.RegisterAndPurchaseProduct("Item0", RewardForItem0);
    }

    private void RewardForItem0(){
        Debug.Log("Successfully purchased Item 0");
    }
    // ...
}
```

### **No Ads**
- `NoAds` class is just a syntax sugar to help your no ads integration.

```C#
using UnityEngine;
using MondayOFF;

public class SampleScript: MonoBehaviour {
    // ...
    public void NoAdsCallback() {
    // (Optional)
    // NoAds disables interstitial and banner by default. Add your own functionality if needed
        NoAds.OnNoAds += ()=>Debug.Log("[EVERYDAY] Bought No Ads");
    }

    public void BuyNoAds() {
        // It is same as IAPManager.PurchaseProduct(NoAds.NoAdsProductKey);
        NoAds.Purchase();
    }
    // ...
}
```


# Known Issues
- Facebook SDK 14.1.0 has an bug which prints `Configuring Facebook SDK dlls for each platform` to console whenever asset is chagned or scripts are recompiled. It is annoying but does not affect gameplay or development.