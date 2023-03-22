7 easy steps to get started
This tutorial will show you seven easy steps to add our Unity SDK to your project. We will guide you through how to sign up, register an app, import the SDK and place your first ad unit in your app.
Prerequisites:
- Installed Unity versions 2019.1 or newer
- Adverty Unity SDK version 4.1.4
- Some familarity with Unity


Step 1: Sign-up and Log in
Go to Adverty.com and signup as a publisher by clicking on "Sign up" in the top right corner.
When you have signed up on adverty.com you will receive an email to proceed with the account creation.
When your account is created, log in and go to the Apps page by clicking on "Apps" in the left sidebar, which is where you manage your apps.


Step 2: Add your App
Add your app in the App tabs and follow the steps presented to you. During the process,
you will get an API-key that you will need to enter in our Unity plugin later. You can at any time retrieve your API-key from the portal.


Step 3: Get the SDK
Next go to "Resources", located in the left side bar. Click on the icon that directs to the Adverty SDK.
You will get to our repository. Download our unitypackage and import it to your project.


Step 4: Add your API-key to the SDK
Go to the Tools->Adverty Menu and open the Settings tab.
Paste in your application key and choose the correct platform for your application. (AR: Mobile AR, VR: Mobile and Desktop VR).


Step 5: Use sample script to start Adverty
Start an Adverty session when your app has been launched by calling Adverty.Init with UserData parameter.
Also assign your GameCamera to AdvertySettings. AdvertySDK incorporates camera and rendering calculations on the main camera.
This is important to ensure revenue for your ad units.
Example:
using UnityEngine;
using Adverty;

public class InitializeAdvertySDK : MonoBehaviour
{
    private void Start()
    {
        //Create UserData and initialize Adverty SDK
        UserData userData = new UserData(AgeSegment.Unknown, Gender.Unknown);
        AdvertySDK.Init(userData);

       Adverty.AdvertySettings.SetMainCamera(YourGameCamera);
    }
}


Step 6: Place an Ad Unit to the Scene
Navigate to Adverty → Prefabs and place either a InPlayUnit or a InMenuUnit based prefab
into the scene view (InMenu unit needs to be added to a Canvas).
Adjust its size and shape through the properties in the Unit component.
You can change the size and on InPlay units also have the ability to
change ratio and bend the unit to fit pillars and other cylindrical objects.
Remember to fill in the Context tags of each Unit to enable us to target with relevant ads.
Place the adunits where they are visible to your users. SDK tracks the viewability of each adunit.
To make sure your adunits are viewable you can use the ImpViz component, see documentation for details.


 Step 7: Press run and you will receive your first Ad 
Press play inside your editor and you will receive your first sample ad!
If you don't get an ad, it could be because of these reasons:
- Your device doesn't support compute shaders (On Mac, make sure to run Metal editor)
- Unity editor emulates a lower Graphics API. Go to Edit → Graphics Emulation and select "no emulation".

More information is available in the Documentation

