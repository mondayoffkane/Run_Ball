using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupDemo : UI_Popup
{
    enum Images
    {
        Back,
    }
    enum Buttons
    {
        test,
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        GetButton(Buttons.test).AddButtonEvent(() => ClosePop(GetImage(Images.Back).transform, null, true));

        OpenPop(GetImage(Images.Back).transform);


    }
}
