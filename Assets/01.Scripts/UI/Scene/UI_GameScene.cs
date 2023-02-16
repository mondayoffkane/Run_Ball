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
    }

    enum Buttons
    {
        AddBall,
        MergeBalls,
        AddPin
    }

    enum Texts
    {
        Money_Text
    }

    public Text MoneyText;

    public Text AddBallText, MergeBallsText, AddPinText;
    public Button AddBall_Button, MergeBalls_Button, AddPin_Button;


    private void Awake()
    {
        Init();
        Managers._uiGameScene = this;

        //Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Text>(typeof(Texts));
        //SetJoyStick();

        // Managers.GameInit();

        //GetImage(Images.test).gameObject.BindEvent(() => Managers.UI.ShowPopupUI<UI_PopupDemo>());

        SetButton();
    }

    private void SetJoyStick()
    {
        //GetImage(Images.JoyStick).GetComponent<JoyStickController>().Init(GetImage(Images.JoyStick));
    }


    void SetButton()
    {
        AddBall_Button = GetButton(Buttons.AddBall);
        MergeBalls_Button = GetButton(Buttons.MergeBalls);
        AddPin_Button = GetButton(Buttons.AddPin);

        AddBall_Button.AddButtonEvent(() => Managers.Game.AddBall());
        MergeBalls_Button.AddButtonEvent(() => Managers.Game.MergeBalls());
        AddPin_Button.AddButtonEvent(() => Managers.Game.AddPin());

        MoneyText = GetText(Texts.Money_Text);

        AddBallText = AddBall_Button.transform.GetChild(0).GetComponent<Text>();
        MergeBallsText = MergeBalls_Button.transform.GetChild(0).GetComponent<Text>();
        AddPinText = AddPin_Button.transform.GetChild(0).GetComponent<Text>();

    }
    public void TestFunc()
    {

    }

}
