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

    public double Money;
    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    public GameObject[] Stages;
    public int Max_Stage = 3;
    public Shooter _currentShooter;

    // ===================================
    public void Init()
    {
        NumberMeshs = new Mesh[10];
        for (int i = 0; i < 10; i++)
        {
            NumberMeshs[i] = Resources.Load<Mesh>("Number/" + i);
        }

        InitStage();

    }
    public void Clear()
    {
        joyStickController.DownAction = null;
        joyStickController.UpAction = null;
        joyStickController.JoystickMoveAction = null;
    }

    public void InitStage()
    {
        Stages = new GameObject[Max_Stage];

        for (int i = 0; i < Max_Stage; i++)
        {
            Stages[i] = Instantiate(Resources.Load<GameObject>("Stage_" + i));
        }

        SetStage();
    }

    public void SetStage(int _level = 0)
    {
        for (int i = 0; i < Max_Stage; i++)
        {
            Stages[i].SetActive(false);
        }

        GameObject _stage = Stages[_level % Max_Stage];
        _stage.SetActive(true);

        _currentShooter = _stage.GetComponent<GridManager>()._shooter;

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





    public static string ToCurrencyString(double number, int _num = 0)
    {
        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            //Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);
            switch (_num)
            {
                case 0:
                    showNumber = temp.ToString("F1").Replace(".0", "");
                    break;

                case 1:
                    if (remainder == 2)
                    {
                        showNumber = temp.ToString("F0").Replace(".0", "");
                    }
                    else
                    {
                        showNumber = temp.ToString("F1").Replace(".0", "");
                    }

                    break;

                case 2: //  소수 둘째자리까지만 출력한다.
                    showNumber = temp.ToString("F2").Replace(".0", "");
                    break;

                case 3:
                    showNumber = temp.ToString("F0").Replace(".0", "");
                    break;
            }


        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }

    public void AddBall()
    {
        _currentShooter.AddBall();
    }


}
