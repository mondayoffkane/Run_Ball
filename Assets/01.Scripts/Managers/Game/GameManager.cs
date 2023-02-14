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

    public bool isPick = false;

    public Transform Pick_Obj;

    public Vector3 mousePos;
    public Vector3 dir;

    [SerializeField] RaycastHit[] hits;

    public Transform Temp_Pin;
    public Transform Temp_Prev_Point;
    public Transform Temp_Next_Point;

    public GridManager _gridManager;
    // ===================================
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




    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Transform _obj = Instantiate(_gridManager.Object_Pref).transform;
            Point _point = _gridManager.FindEmptyPoint(GridManager.FindState.Random);
            _point.Fix_Pin = _obj;
            _obj.transform.position = _point.transform.position;
            _obj.GetComponent<Pin>().SetPin((Pin.PinType)Random.Range(0, 3));
            _obj.GetComponent<Pin>().Prev_Point = _point.transform;

        }


        if (Input.GetMouseButtonDown(0))
        {

            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
            dir = new Vector3(dir.x, dir.y, 50f);

            hits = Physics.RaycastAll(mousePos, dir);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Pin"))
                {
                    Temp_Pin = hits[i].transform;
                }
                else if (hits[i].transform.CompareTag("Point"))
                {
                    Temp_Prev_Point = hits[i].transform;
                }
            }

            if (Temp_Pin != null && Temp_Prev_Point != null)
            {
                isPick = true;
                Pick_Obj = Temp_Pin;
                Pick_Obj.gameObject.layer = 2;
                Pick_Obj.GetComponent<Pin>().Prev_Point = Temp_Prev_Point;
            }
            else
            {
                Temp_Pin = null; Temp_Prev_Point = null;
                isPick = false;
            }


        }
        else if (Input.GetMouseButton(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
            dir = new Vector3(dir.x, dir.y, 50f);
            Debug.DrawRay(mousePos, dir);

            if (isPick)
            {
                Pick_Obj.position = new Vector3(mousePos.x, mousePos.y, 0f);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isPick)
            {
                mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
                dir = new Vector3(dir.x, dir.y, 50f);
                hits = Physics.RaycastAll(mousePos, dir);


                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("Point"))
                    {
                        Temp_Next_Point = hits[i].transform;
                    }
                }
                if (Temp_Next_Point != null)
                {
                    if (Temp_Next_Point.GetComponent<Point>().Fix_Pin == null)
                    {
                        Pick_Obj.GetComponent<Pin>().Prev_Point = Temp_Next_Point;
                        Temp_Prev_Point.GetComponent<Point>().Fix_Pin = null;
                        Temp_Next_Point.GetComponent<Point>().Fix_Pin = Pick_Obj;
                        Pick_Obj.transform.position = Temp_Next_Point.position;

                    }
                    else
                    {
                        Pick_Obj.position = Temp_Prev_Point.position;
                    }
                }
                else
                {
                    Pick_Obj.position = Temp_Prev_Point.position;
                }


                Pick_Obj.gameObject.layer = 0;
                isPick = false;

            }
            Pick_Obj = null;
            Temp_Pin = null;
            Temp_Prev_Point = null;
            Temp_Next_Point = null;
        }
    }
}
