using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
//using UnityEditor.iOS;
using UnityEditor;
using Unity.VisualScripting;

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


    public bool isPick = false;
    Transform Pick_Obj;
    Vector3 mousePos;
    Vector3 dir;
    RaycastHit[] hits;

    Transform Temp_Pin;
    Transform Temp_Prev_Point;
    Transform Temp_Next_Point;

    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    GameObject[] Stages;
    int Max_Stage = 7;
    public int Current_Stage_Level;

    List<Ball> ballList;
    List<Ball> tempMergeBalls;
    [ShowInInspector]
    List<Pin> pinList;
    int[] ballLevelCount;
    public double[] ballBasePrice = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public double[] addBall_BasePrice = new double[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };
    public double[] mergeBalls_BasePrice = new double[] { 30, 40, 50, 60, 70, 80, 90, 100, 110, 120 };
    public double[] addPin_BasePrice = new double[] { 20, 30, 40, 50, 60, 70, 80, 90, 100, 110 };
    public double[] ClearMoney = new double[] { 1000, 2000, 3000, 4500, 7000, 8000, 10000, 20000, 30000, 50000 };
    public double StageScope = 1.2d;
    [SerializeField] double currentClearMoney = 0;
    [SerializeField] bool isRunning = true;

    public GridManager _gridManager;
    public Shooter _currentShooter;
    public double Money;

    public int addBall_Level = 1;
    public int mergeBalls_Level = 1;
    public int addPin_Level = 1;

    public Material[] SkyBox_Mats = new Material[7];

    // ===================================
    public void Init()
    {
        ballList = new List<Ball>();
        tempMergeBalls = new List<Ball>();
        ballLevelCount = new int[10];
        pinList = new List<Pin>();

        InitStage();

        SkyBox_Mats = new Material[7];
        SkyBox_Mats = Resources.LoadAll<Material>("SkyBox");

    }
    public void Clear()
    {
        joyStickController.DownAction = null;
        joyStickController.UpAction = null;
        joyStickController.JoystickMoveAction = null;
    }

    public void InitStage()
    {
        _currentShooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<Shooter>();
        Stages = new GameObject[Max_Stage];

        for (int i = 0; i < Max_Stage; i++)
        {
            Stages[i] = Instantiate(Resources.Load<GameObject>("Stage_" + i));
        }

        LoadData();

        SetStage(Current_Stage_Level);
    }

    public void SetStage(int _level = 0)
    {
        for (int i = 0; i < Max_Stage; i++)
        {
            Stages[i].SetActive(false);
        }

        GameObject _stage = Stages[_level % Max_Stage];
        _stage.SetActive(true);

        //_currentShooter = _stage.GetComponent<GridManager>()._shooter;

        if (ballList.Count > 0)
        {
            foreach (Ball _ball in ballList)
            {
                Managers.Pool.Push(_ball.GetComponent<Poolable>());
            }
        }
        ballList.Clear();

        if (pinList.Count > 0)
        {
            foreach (Pin _pin in pinList)
            {
                Managers.Pool.Push(_pin.GetComponent<Poolable>());
            }
        }
        pinList.Clear();
        ballLevelCount = new int[10];

        addBall_Level = 1;
        mergeBalls_Level = 1;
        addPin_Level = 1;
        isRunning = true;

        Invoke("StartStage", 0.5f);

        Managers._uiGameScene.GuageText.text = $"{ToCurrencyString(currentClearMoney)} / {ToCurrencyString(ClearMoney[Current_Stage_Level & Max_Stage])}";
        Managers._uiGameScene.FillGuage.fillAmount = (float)(currentClearMoney / ClearMoney[Current_Stage_Level & Max_Stage]);



    }

    public void StartStage()
    {
        Camera.main.transform.GetComponent<Skybox>().material =
           //_stage.GetComponent<GridManager>().SkyBox_Mat;
           SkyBox_Mats[Current_Stage_Level % Max_Stage];

        // 스테이지 시작시 최초 1회 제공
        for (int i = 0; i < 3; i++)
        {
            Ball _newBall = _currentShooter.AddBall();
            _currentShooter.Ball_Queue.Enqueue(_newBall.GetComponent<Rigidbody>());
            ballList.Add(_newBall);
            _newBall.Init();
            CheckMergeList();

        }

        for (int i = 0; i < 1; i++)
        {
            if (_gridManager.Pin_Pref == null) _gridManager.Pin_Pref = Resources.Load<GameObject>("Pin");

            Pin _pin = Managers.Pool.Pop(_gridManager.Pin_Pref).GetComponent<Pin>();

            Point _point = _gridManager.FindEmptyPoint(GridManager.FindState.Random);
            _point.Fix_Pin = _pin.transform;
            _pin.transform.position = _point.transform.position;
            _pin.GetComponent<Pin>().SetPin((Pin.PinType)Random.Range(0, 3));
            _pin.GetComponent<Pin>().Prev_Point = _point.transform;
            pinList.Add(_pin);



        }
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBall();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            MergeBalls();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AddPin();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            AddPin(1);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            AddPin(2);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            AddPin(3);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Money += 10000;
            MoneyUpdate();
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ES3.DeleteFile();
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

                        Temp_Prev_Point.GetComponent<Renderer>().enabled = true;
                        Temp_Next_Point.GetComponent<Renderer>().enabled = false;

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
        Money -= /*_gridManager.addBall_BasePrice*/ addBall_BasePrice[Current_Stage_Level % Max_Stage] * addBall_Level * StageScope * (Current_Stage_Level + 1);
        addBall_Level++;

        Ball _newBall = _currentShooter.AddBall();
        ballList.Add(_newBall);
        _newBall.Init();

        CheckMergeList(); // 위치 변경 예정 , 돈 체크도 같이 해야함

        MoneyUpdate();


    }

    public void MergeBalls()
    {

        Money -= mergeBalls_BasePrice[Current_Stage_Level % Max_Stage] * mergeBalls_Level * StageScope * (Current_Stage_Level + 1);    // _gridManager.mergeBalls_BasePrice * mergeBalls_Level;
        mergeBalls_Level++;

        int tempLevel = 0;
        foreach (Ball _ball in tempMergeBalls)
        {
            tempLevel = _ball.Level;
            Managers.Pool.Push(_ball.GetComponent<Poolable>());
            ballList.Remove(_ball);
        }
        tempMergeBalls.Clear();
        Ball _newBall = _currentShooter.AddBall();
        _newBall.Init(tempLevel + 1);
        ballList.Add(_newBall);
        _currentShooter.MergeShoot(_newBall);
        CheckMergeList();

        MoneyUpdate();
    }

    public void AddPin(int _num = 0)
    {
        Money -= addPin_BasePrice[Current_Stage_Level % Max_Stage] * addPin_Level * StageScope * (Current_Stage_Level + 1);  //_gridManager.addPin_BasePrice * addPin_Level;
        addPin_Level++;

        //Transform _obj = Instantiate(_gridManager.Pin_Pref).transform;
        Pin _pin = Managers.Pool.Pop(_gridManager.Pin_Pref).GetComponent<Pin>();

        Point _point = _gridManager.FindEmptyPoint(GridManager.FindState.Random);
        _point.Fix_Pin = _pin.transform;
        _pin.transform.position = _point.transform.position;
        switch (_num)
        {
            case 0:
                _pin.GetComponent<Pin>().SetPin((Pin.PinType)Random.Range(0, 3));
                break;

            case 1:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Diamond);
                break;

            case 2:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Rot);
                break;

            case 3:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Spring);
                break;
            default:
                break;
        }

        _pin.GetComponent<Pin>().Prev_Point = _point.transform;
        pinList.Add(_pin);
        MoneyUpdate();
        Managers.Sound.Play(Resources.Load<AudioClip>("Sound/Spawn"));
    }

    public void CheckMergeList()
    {
        for (int i = 0; i < ballLevelCount.Length; i++)
        {
            ballLevelCount[i] = 0;
        }

        foreach (Ball _ball in ballList)
        {
            ballLevelCount[_ball.Level]++;
        }

        for (int level = 0; level < ballLevelCount.Length; level++)
        {
            if (ballLevelCount[level] >= 3)
            {
                foreach (Ball _ball in ballList)
                {
                    if (_ball.Level == level && tempMergeBalls.Count < 3)
                    {
                        tempMergeBalls.Add(_ball);
                    }
                }
                break;
            }
        }

    }

    public void MoneyUpdate()
    {
        Managers._uiGameScene.MoneyText.text = $"${ToCurrencyString(Money)}";
        CheckButtons();
    }

    public void CheckButtons()
    {
        // 버튼 텍스트 업데이트
        Managers._uiGameScene.AddBallText.text = $"${ToCurrencyString(addBall_BasePrice[Current_Stage_Level % Max_Stage] * addBall_Level * StageScope * (Current_Stage_Level + 1) /*_gridManager.addBall_BasePrice * addBall_Level*/)}";
        Managers._uiGameScene.MergeBallsText.text = $"${ToCurrencyString(mergeBalls_BasePrice[Current_Stage_Level % Max_Stage] * mergeBalls_Level * StageScope * (Current_Stage_Level + 1) /*_gridManager.mergeBalls_BasePrice * mergeBalls_Level*/)}";
        Managers._uiGameScene.AddPinText.text = $"${ToCurrencyString(addPin_BasePrice[Current_Stage_Level % Max_Stage] * addPin_Level * StageScope * (Current_Stage_Level + 1) /*_gridManager.addPin_BasePrice * addPin_Level*/)}";


        // 금액 비교후 버튼 활성화 결정

        Managers._uiGameScene.AddBall_Button.interactable
            = Money >= addBall_BasePrice[Current_Stage_Level % Max_Stage] * addBall_Level * StageScope * (Current_Stage_Level + 1)
            //_gridManager.addBall_BasePrice * addBall_Level
            ? true : false;

        Managers._uiGameScene.MergeBalls_Button.interactable
             = (Money >= mergeBalls_BasePrice[Current_Stage_Level % Max_Stage] * mergeBalls_Level * StageScope * (Current_Stage_Level + 1) /*_gridManager.mergeBalls_BasePrice * mergeBalls_Level*/)
             && (tempMergeBalls.Count >= 3) ? true : false;

        Managers._uiGameScene.AddPin_Button.interactable
        = (Money >= addPin_BasePrice[Current_Stage_Level % Max_Stage] * addPin_Level * StageScope * (Current_Stage_Level + 1))
        && (_gridManager.FindEmptyPoint(GridManager.FindState.Random) != null)
        //_gridManager.addPin_BasePrice * addPin_Level
        ? true : false;



    }

    public void AddMoney(double _money)
    {
        if (isRunning)
        {
            Money += _money;
            MoneyUpdate();

            currentClearMoney += _money;
            Managers._uiGameScene.GuageText.text = $"{ToCurrencyString(currentClearMoney)} / {ToCurrencyString(ClearMoney[Current_Stage_Level & Max_Stage])}";
            Managers._uiGameScene.FillGuage.fillAmount = (float)(currentClearMoney / ClearMoney[Current_Stage_Level & Max_Stage]);
            if (currentClearMoney >= /*_gridManager.ClearMoney*/ ClearMoney[Current_Stage_Level & Max_Stage])
            {
                StageClear();
            }
        }
    }

    public void StageClear()
    {
        isRunning = false;
        Current_Stage_Level++;

        Managers.Sound.Play(Resources.Load<AudioClip>("Sound/Clear"));

        Debug.Log("Stage Clear!");
        Managers._uiGameScene.Upgrade_Panel.SetActive(false);
        Managers._uiGameScene.Clear_Panel.SetActive(true);

        SaveData();


        // Enable Clear UI
        // Hide Upgrade UI
    }

    public void NextStage_Button()
    {
        isRunning = false;
        currentClearMoney = 0;
        //Managers._uiGameScene.FillGuage.fillAmount = 0f;
        //Managers._uiGameScene.GuageText.text = $"{ToCurrencyString(currentClearMoney)} / {ToCurrencyString(ClearMoney[Current_Stage_Level & Max_Stage])}";


        Managers._uiGameScene.Upgrade_Panel.SetActive(true);
        Managers._uiGameScene.Clear_Panel.SetActive(false);

        SetStage(Current_Stage_Level);
        // add new stages
    }


    #region Data Func

    public void LoadData()
    {
        Money = ES3.Load<double>("Money", 0);
        Current_Stage_Level = ES3.Load<int>("StageLevel", 0);
    }


    public void SaveData()
    {
        ES3.Save<int>("StageLevel", Current_Stage_Level);
        ES3.Save<double>("Money", Money);

    }


    #endregion
}
