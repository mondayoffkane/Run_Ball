using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Images
    {
        test,
        JoyStick,
        FillGuage
    }

    enum Buttons
    {
        AddBall,
        MergeBalls,
        AddPin,
        NextStage
    }

    enum Texts
    {
        Money_Text,
        Guage_Text

    }

    enum GameObjects
    {
        Base_Panel,
        Upgrade_Panel,
        Clear_Panel
    }

    public Text MoneyText;

    public Text AddBallText, MergeBallsText, AddPinText, GuageText;
    public Button AddBall_Button, MergeBalls_Button, AddPin_Button, NextStage_Button;

    public GameObject Base_Panel, Upgrade_Panel, Clear_Panel;
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

        FillGuage = GetImage(Images.FillGuage);


    }
    public void TestFunc()
    {

    }

}
