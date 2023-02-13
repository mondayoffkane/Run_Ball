using UnityEngine;
using Sirenix.OdinInspector;
public class GameManager : SerializedMonoBehaviour
{
    public JoyStickController joyStickController;//{ private get; set; }
    public void SetDownAction(System.Action action)
    {
        joyStickController.AddDownEvent(action);
    }
    public void SetUpAction(System.Action action)
    {
        joyStickController.AddUpEvent(action);
    }
    public void SetMoveAction(System.Action<Vector2> action)
    {
        joyStickController.AddMoveEvent(action);
    }


    public Mesh[] NumberMeshs;


    public void Init()
    {
        NumberMeshs = new Mesh[10];
        for (int i = 0; i < 10; i++)
        {
            NumberMeshs[i] = Resources.Load<Mesh>("Number/" + i);
        }

    }
    public void Clear()
    {
        joyStickController.DownAction = null;
        joyStickController.UpAction = null;
        joyStickController.JoystickMoveAction = null;
    }
}
