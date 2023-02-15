using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    private void Awake()
    {
        Init();

        Bind<UnityEngine.UI.Image>(typeof(Images));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));
        Bind<UnityEngine.UI.Text>(typeof(Texts));

        SetJoyStick();

        // Managers.GameInit();

        GetImage(Images.test).gameObject.BindEvent(() => Managers.UI.ShowPopupUI<UI_PopupDemo>());

        SetButton();
    }

    private void SetJoyStick()
    {
        GetImage(Images.JoyStick).GetComponent<JoyStickController>().Init(GetImage(Images.JoyStick));
    }


    void SetButton()
    {
        GetButton(Buttons.AddBall).AddButtonEvent(() =>
        {
            Managers.Game.AddBall();

            Debug.Log("Button Click");
        });

    }

}
