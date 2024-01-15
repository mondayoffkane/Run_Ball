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
        Restore,
        RV_Merge_Reward,
        Close_Merge_Reward,
        Bonus_Stage,
        Close_Bonus,
        NoAds_On,
        Purchase_NoAds

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
        Setting_Panel,
        Merge_RV_Panel,
        Tutorial_Panel,
        Move_Pin,
        Rotate_Pin,
        Click_Button,
        Remove_Pin,
        Bonus_Stage_Panel,
        NoAds_Panel

    }

    public Text MoneyText;

    public Text AddBallText, MergeBallsText, AddPinText, GuageText, StageText, MPSText;
    public Button AddBall_Button, MergeBalls_Button, AddPin_Button, NextStage_Button
        , RV_AddMoney_Button, RV_AddBalls_Button, RV_DoubleMoney_Button, BallReset_Button
        , Setting_Button, Setting_Close_Button, Sound_Button, Haptic_Button, Restore_Button
        , RV_Merge_Reward, Close_Merge_Reward, Bonus_Stage_Button, Close_Bonus_Button
        , NoAds_On, Purchase_NoAds;

    public GameObject Base_Panel, Upgrade_Panel, Clear_Panel, Store_Panel, Setting_Panel, Merge_RV_Panel
        , Tutorial_Panel, Move_Pin, Rotate_Pin, Click_Button, Remove_Pin, Bonus_Stage_Panel
        , NoAds_Panel;

    public Image FillGuage;

    public RawImage[] Ball_Render_Imgs;


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

        Merge_RV_Panel = GetObject(GameObjects.Merge_RV_Panel);
        Ball_Render_Imgs = Merge_RV_Panel.transform.GetComponentsInChildren<RawImage>();

        RV_Merge_Reward = GetButton(Buttons.RV_Merge_Reward);
        Close_Merge_Reward = GetButton(Buttons.Close_Merge_Reward);

        RV_Merge_Reward.AddButtonEvent(() => AdsManager.ShowRewarded(() => Managers.Game.RV_Merge_Reward()));
        Close_Merge_Reward.AddButtonEvent(() => Merge_RV_Panel.SetActive(false));


        /// /////// Add Tutorial///////////////

        Tutorial_Panel = GetObject(GameObjects.Tutorial_Panel);
        Move_Pin = GetObject(GameObjects.Move_Pin);
        Rotate_Pin = GetObject(GameObjects.Rotate_Pin);
        Click_Button = GetObject(GameObjects.Click_Button);
        Remove_Pin = GetObject(GameObjects.Remove_Pin);

        // Bonus Stage
        Bonus_Stage_Panel = GetObject(GameObjects.Bonus_Stage_Panel);
        Bonus_Stage_Button = GetButton(Buttons.Bonus_Stage);
        Close_Bonus_Button = GetButton(Buttons.Close_Bonus);

        Bonus_Stage_Button.AddButtonEvent(() =>
        {
            Managers.Game.BonusStage();
            Bonus_Stage_Panel.SetActive(false);
        });
        Close_Bonus_Button.AddButtonEvent(() => Bonus_Stage_Panel.SetActive(false));

        Purchase_NoAds = GetButton(Buttons.Purchase_NoAds);
        Purchase_NoAds.AddButtonEvent(() =>
        {
            MondayOFF.NoAds.Purchase();
        });
        MondayOFF.NoAds.OnNoAds += () =>
        {

            Managers.Game.isNoAds = 1;
            PlayerPrefs.SetInt("isNoAds", Managers.Game.isNoAds);

            NoAds_Panel.SetActive(false);
            NoAds_On.gameObject.SetActive(false);

            Debug.Log("구매 완료!");

        };

        NoAds_On = GetButton(Buttons.NoAds_On);
        NoAds_Panel = GetObject(GameObjects.NoAds_Panel);
    }
    public void TestFunc()
    {

    }

}
