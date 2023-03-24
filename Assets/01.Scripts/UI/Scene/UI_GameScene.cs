using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MondayOFF;

public class UI_GameScene : UI_Scene
{
    enum Images
    {
        FillGuage
    }

    enum Buttons
    {
        AddBall,
        MergeBalls,
        AddPin,
        NextStage,
        RV_AddMoney,
        RV_AddBalls,
        RV_DoubleMoney,
        BallReset,
        Setting,
        Setting_Close,
        Sound,
        Haptic,
        Restore

    }

    enum Texts
    {
        Money_Text,
        Guage_Text,
        Stage_Text,
        MPS_Text

    }

    enum GameObjects
    {
        Base_Panel,
        Upgrade_Panel,
        Clear_Panel,
        Store_Panel,
        Setting_Panel
    }

    public Text MoneyText;

    public Text AddBallText, MergeBallsText, AddPinText, GuageText, StageText, MPSText;
    public Button AddBall_Button, MergeBalls_Button, AddPin_Button, NextStage_Button
        , RV_AddMoney_Button, RV_AddBalls_Button, RV_DoubleMoney_Button, BallReset_Button
        , Setting_Button, Setting_Close_Button, Sound_Button, Haptic_Button, Restore_Button;

    public GameObject Base_Panel, Upgrade_Panel, Clear_Panel, Store_Panel, Setting_Panel;
    public Image FillGuage;


    private void Awake()
    {
        Init();
        Managers._uiGameScene = this;

        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<UnityEngine.UI.Image>(typeof(Images));


        SetButton();
    }




    void SetButton() // set buttons and uis
    {
        // Base Upgrade Button
        AddBall_Button = GetButton(Buttons.AddBall);
        MergeBalls_Button = GetButton(Buttons.MergeBalls);
        AddPin_Button = GetButton(Buttons.AddPin);
        NextStage_Button = GetButton(Buttons.NextStage);


        AddBall_Button.AddButtonEvent(() => Managers.Game.AddBall());
        MergeBalls_Button.AddButtonEvent(() => Managers.Game.MergeBalls());
        AddPin_Button.AddButtonEvent(() => Managers.Game.AddPin());
        NextStage_Button.AddButtonEvent(() => Managers.Game.NextStage_Button());

        MoneyText = GetText(Texts.Money_Text);

        AddBallText = AddBall_Button.transform.GetChild(0).GetComponent<Text>();
        MergeBallsText = MergeBalls_Button.transform.GetChild(0).GetComponent<Text>();
        AddPinText = AddPin_Button.transform.GetChild(0).GetComponent<Text>();
        GuageText = GetText(Texts.Guage_Text);

        Base_Panel = GetObject(GameObjects.Base_Panel);
        Upgrade_Panel = GetObject(GameObjects.Upgrade_Panel);
        Clear_Panel = GetObject(GameObjects.Clear_Panel);
        Store_Panel = GetObject(GameObjects.Store_Panel);

        FillGuage = GetImage(Images.FillGuage);
        /////
        // RV Button
        RV_AddMoney_Button = GetButton(Buttons.RV_AddMoney);
        RV_AddBalls_Button = GetButton(Buttons.RV_AddBalls);
        RV_DoubleMoney_Button = GetButton(Buttons.RV_DoubleMoney);

        RV_AddMoney_Button.AddButtonEvent(() => AdsManager.ShowRewarded(() => Managers.Game.RV_AddMoney()));
        RV_AddBalls_Button.AddButtonEvent(() => AdsManager.ShowRewarded(() => Managers.Game.RV_AddBall()));
        RV_DoubleMoney_Button.AddButtonEvent(() => AdsManager.ShowRewarded(() => Managers.Game.RV_DoubleMoney()));

        BallReset_Button = GetButton(Buttons.BallReset);
        BallReset_Button.AddButtonEvent(() => Managers.Game.ResetBalls());

        // ///////// Stage, MPS
        StageText = GetText(Texts.Stage_Text);
        MPSText = GetText(Texts.MPS_Text);

        // //////// Setting buttons
        Setting_Panel = GetObject(GameObjects.Setting_Panel);
        Setting_Button = GetButton(Buttons.Setting);
        Setting_Close_Button = GetButton(Buttons.Setting_Close);
        Sound_Button = GetButton(Buttons.Sound);
        Haptic_Button = GetButton(Buttons.Haptic);
        Restore_Button = GetButton(Buttons.Restore);

        Setting_Button.AddButtonEvent(() =>
        {
            Managers.Game.GameObjectOnOnff(Setting_Panel);
            Sound_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Img/Sound_" + Managers.Data.UseSound);
            Haptic_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Img/Haptic_" + Managers.Data.UseHaptic);
        });
        Setting_Close_Button.AddButtonEvent(() => Managers.Game.GameObjectOnOnff(Setting_Panel));
        Sound_Button.AddButtonEvent(() =>
        {
            Managers.Data.UseSound = !Managers.Data.UseSound;
            Sound_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Img/Sound_" + Managers.Data.UseSound);
            Debug.Log("Img/Sound_" + Managers.Data.UseSound);

        });
        Haptic_Button.AddButtonEvent(() =>
        {
            Managers.Data.UseHaptic = !Managers.Data.UseHaptic;
            Haptic_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("Img/Haptic_" + Managers.Data.UseHaptic);
            Debug.Log("Img/Haptic_" + Managers.Data.UseHaptic);
        });

        Restore_Button.AddButtonEvent(() => MondayOFF.IAPManager.RestorePurchase());



    }
    public void TestFunc()
    {

    }

}
