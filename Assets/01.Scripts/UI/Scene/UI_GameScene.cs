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

    private void Awake()
    {
        Init();

        Bind<UnityEngine.UI.Image>(typeof(Images));

        SetJoyStick();

        // Managers.GameInit();

        GetImage(Images.test).gameObject.BindEvent(() => Managers.UI.ShowPopupUI<UI_PopupDemo>());
    }

    private void SetJoyStick()
    {
        GetImage(Images.JoyStick).GetComponent<JoyStickController>().Init(GetImage(Images.JoyStick));
    }

}
