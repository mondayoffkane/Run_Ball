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
    [SerializeField] Vector3 mousePos;
    [SerializeField] Vector3 dir;
    [SerializeField] RaycastHit[] hits;

    Transform Temp_Pin;
    Transform Temp_Prev_Point;
    Transform Temp_Next_Point;

    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    //GameObject[] Stages;
    int Max_Stage = 10;
    public int Current_Stage_Level;
    public GameObject Current_Stage;


    public List<Ball> ballList;
    [SerializeField] List<Ball> tempMergeBalls;
    [ShowInInspector]
    List<Pin> pinList;
    public int[] ballLevelCount;

    public double ballBasePrice = 1d;
    public double addBall_BasePrice = 5d;
    public double mergeBalls_BasePrice = 10d;
    public double addPin_BasePrice = 20d;

    public double ball_PriceScope = 1.2d;
    public double addBall_PriceScope = 5d;
    public double mergeBalls_PriceScope = 10d;
    public double addPin_PriceScope = 20d;

    public double ClearMoney = 0d;
    public double StageScope = 1.2d;
    public double currentClearMoney = 0;
    [SerializeField] bool isRunning = true;

    public GridManager _gridManager;
    public Shooter _currentShooter;
    public double Money;

    public int addBall_Level = 1;
    public int mergeBalls_Level = 1;
    public int addPin_Level = 1;

    public Material[] SkyBox_Mats = new Material[7];

    public Vector3 startMousePos, offset, _rotation;
    public Transform _rotObj;


    [SerializeField] double tempAddBall_Price, tempMergeBalls_Price, tempAddPin_Price;

    // ===================================
    public void Init()
    {
        CheckMaxStage();


        ballList = new List<Ball>();
        tempMergeBalls = new List<Ball>();
        ballLevelCount = new int[10];
        pinList = new List<Pin>();

        InitStage();

        SkyBox_Mats = new Material[7];
        SkyBox_Mats = Resources.LoadAll<Material>("SkyBox");

    }

    void CheckMaxStage()
    {
        GameObject[] _stages = Resources.LoadAll<GameObject>("Stages");
        Max_Stage = _stages.Length;

    }


    public void Clear()
    {
        joyStickController.DownAction = null;
        joyStickController.UpAction = null;
        joyStickController.JoystickMoveAction = null;
    }

    public void InitStage()
    {
        //_currentShooter = GameObject.FindGameObjectWithTag("Shooter").GetComponent<Shooter>();
        //Stages = new GameObject[Max_Stage];

        //for (int i = 0; i < Max_Stage; i++)
        //{
        //    Stages[i] = Instantiate(Resources.Load<GameObject>("Stage_" + i));
        //}

        LoadData();

        SetStage(Current_Stage_Level);
    }

    public void SetStage(int _level = 0)
    {
        //for (int i = 0; i < Max_Stage; i++)
        //{
        //    Stages[i].SetActive(false);
        //}
        if (Current_Stage != null)
        {
            Destroy(Current_Stage);
            Current_Stage = null;
        }
        Current_Stage = Instantiate(Resources.Load<GameObject>("Stages/Stage_" + _level % Max_Stage));
        _gridManager = Current_Stage.GetComponent<GridManager>();
        _currentShooter = _gridManager._shooter;

        //UnityEditor.EditorApplication.isPaused = true;
        //GameObject _stage = Current_Stage;
        //_stage.SetActive(true);

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
        ClearMoney = (ballBasePrice * ball_PriceScope * (double)(Current_Stage_Level + 1)) * 5000d; // Set Stage Clear Money
        currentClearMoney = 0d;
        pinList.Clear();
        ballLevelCount = new int[10];

        addBall_Level = 1;
        mergeBalls_Level = 1;
        addPin_Level = 1;
        isRunning = true;

        Invoke("StartStage", 0.1f);

        Managers._uiGameScene.GuageText.text = $"{ToCurrencyString(currentClearMoney)} / {ToCurrencyString(ClearMoney)}";
        Managers._uiGameScene.FillGuage.fillAmount = (float)(currentClearMoney / ClearMoney);



    }

    public void StartStage()
    {
        Camera.main.transform.GetComponent<Skybox>().material =
           //_stage.GetComponent<GridManager>().SkyBox_Mat;
           SkyBox_Mats[Current_Stage_Level % 7];

        // 스테이지 시작시 최초 1회 제공
        for (int i = 0; i < 3; i++)
        {
            AddBall(false);
        }

        for (int i = 0; i < 1; i++)
        {
            AddPin(0, false);

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
        else if (Input.GetKeyDown(KeyCode.A))
        {
            AddPin(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AddPin(2);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AddPin(3);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            AddPin(4);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            AddPin(5);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ResetBalls();
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            Money += 10000;
            MoneyUpdate();
        }

        else if (Input.GetKeyDown(KeyCode.T))
        {
            Managers._uiGameScene.Store_Panel.SetActive(!Managers._uiGameScene.Store_Panel.activeSelf);
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ES3.DeleteFile();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Current_Stage_Level--;
            if (Current_Stage_Level < 0) Current_Stage_Level = 0;
            SaveData();
            SetStage(Current_Stage_Level);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Current_Stage_Level++;
            //if (Current_Stage_Level > Max_Stage) Current_Stage_Level = Max_Stage;
            SaveData();
            SetStage(Current_Stage_Level);
        }



        if (Input.GetMouseButtonDown(0))
        {

            //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30f));
            //dir = (mousePos - Camera.main.transform.position);
            //Debug.DrawRay(Camera.main.transform.position, dir);

            startMousePos = Input.mousePosition;

            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hits = Physics.RaycastAll(_ray);
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
                else if (hits[i].transform.CompareTag("Handle"))
                {
                    _rotObj = hits[i].transform.parent; //.parent;
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
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30f));

            if (_rotObj != null)
            {
                //offset = (Input.mousePosition - startMousePos);
                //_rotation.z = -(offset.x + offset.y) * Time.deltaTime * 30f;
                //_rotObj.Rotate(_rotation);
                //startMousePos = Input.mousePosition;

                Vector3 _testpos = (mousePos - _rotObj.transform.position).normalized;

                //_rotObj.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.right - _testpos);
                _rotObj.rotation = Quaternion.FromToRotation(Vector3.left, _testpos);

            }

            //dir = (mousePos - Camera.main.transform.position);
            //Debug.DrawRay(Camera.main.transform.position, dir);

            //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f));
            //dir = new Vector3(dir.x, dir.y, 50f);
            //Debug.DrawRay(mousePos, dir);

            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (isPick)
            {
                Pick_Obj.position = new Vector3(mousePos.x, mousePos.y, 0f);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _rotObj = null;
            if (isPick)
            {
                //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30f));
                //dir = (mousePos - Camera.main.transform.position);
                //hits = Physics.RaycastAll(mousePos, dir);

                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(_ray);



                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("Point"))
                    {
                        Temp_Next_Point = hits[i].transform;
                    }
                    else if (hits[i].transform.CompareTag("Delete"))
                    {
                        Managers.Pool.Push(Pick_Obj.GetComponent<Poolable>());
                        

                        Temp_Prev_Point.GetComponent<Point>().Reset_Pin();
                        

                    }

                }
                if (Temp_Next_Point != null)
                {
                    if (Temp_Next_Point.GetComponent<Point>().Fix_Pin == null)
                    {
                        Pick_Obj.GetComponent<Pin>().Prev_Point = Temp_Next_Point;
                        Temp_Prev_Point.GetComponent<Point>().Reset_Pin();
                        //Temp_Prev_Point.GetComponent<Point>().Fix_Pin = null;
                        Temp_Next_Point.GetComponent<Point>().Set_Pin(Pick_Obj);
                        //Temp_Next_Point.GetComponent<Point>().Fix_Pin = Pick_Obj;
                        Pick_Obj.transform.position = Temp_Next_Point.position;

                        //Temp_Prev_Point.GetComponent<Renderer>().enabled = true;
                        //Temp_Next_Point.GetComponent<Renderer>().enabled = false;

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

    public void AddBall(bool isPay = true)
    {
        if (isPay == true)
        {
            Money -= tempAddBall_Price;
            //addBall_BasePrice[Current_Stage_Level % Max_Stage] + addBall_Level * StageScope * (Current_Stage_Level + 1);
            addBall_Level++;
        }

        Ball _newBall = _currentShooter.AddBall();
        _newBall.gameObject.SetActive(false);
        //_currentShooter.Ball_Queue.Enqueue(_newBall.GetComponent<Rigidbody>());
        _currentShooter.Ball_Wait_List.Add(_newBall.GetComponent<Rigidbody>());
        ballList.Add(_newBall);
        _newBall.Init();

        CheckMergeList(); // 위치 변경 예정 , 돈 체크도 같이 해야함

        MoneyUpdate();



    }

    public void MergeBalls()
    {

        Money -= tempMergeBalls_Price;
        //mergeBalls_BasePrice[Current_Stage_Level % Max_Stage] + mergeBalls_Level * StageScope * (Current_Stage_Level + 1);    // _gridManager.mergeBalls_BasePrice * mergeBalls_Level;
        mergeBalls_Level++;

        int tempLevel = 0;
        foreach (Ball _ball in tempMergeBalls)
        {
            tempLevel = _ball.Level;
            Managers.Pool.Push(_ball.GetComponent<Poolable>());
            ballList.Remove(_ball);
            _currentShooter.Ball_Wait_List.Remove(_ball.GetComponent<Rigidbody>());
        }
        tempMergeBalls.Clear();
        Ball _newBall = _currentShooter.AddBall();
        _newBall.Init(tempLevel + 1);
        ballList.Add(_newBall);
        _currentShooter.MergeShoot(_newBall);
        CheckMergeList();

        MoneyUpdate();
    }

    public void AddPin(int _num = 0, bool isPay = true)
    {
        if (isPay == true)
        {
            Money -= tempAddPin_Price;
            //addPin_BasePrice[Current_Stage_Level % Max_Stage] + addPin_Level * StageScope * (Current_Stage_Level + 1);  //_gridManager.addPin_BasePrice * addPin_Level;
            addPin_Level++;
        }

        //Transform _obj = Instantiate(_gridManager.Pin_Pref).transform;
        Pin _pin = Managers.Pool.Pop(_gridManager.Pin_Pref, transform).GetComponent<Pin>();

        Point _point = _gridManager.FindEmptyPoint(GridManager.FindState.Random);

        if (_point == null)
        {
            return;

        }

        _point.Fix_Pin = _pin.transform;
        _pin.transform.position = _point.transform.position;
        _point.GetComponent<Renderer>().enabled = false;
        switch (_num)
        {
            case 0:
                //_pin.GetComponent<Pin>().SetPin((Pin.PinType)Random.Range(0, 3));
                //System.Random _rand = new System.Random();
                _pin.GetComponent<Pin>().SetPin(_gridManager
                    .PinType_Array[new System.Random().Next(0, _gridManager.PinType_Array.Length)]);
                break;


            case 1:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Triangle);
                break;

            case 2:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Square);
                break;

            case 3:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Hex);
                break;

            case 4:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Circle);

                break;

            case 5:
                _pin.GetComponent<Pin>().SetPin(Pin.PinType.Cannon);
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
        Managers._uiGameScene.GuageText.text = $"{ToCurrencyString(currentClearMoney)} / {ToCurrencyString(ClearMoney)}";
        Managers._uiGameScene.FillGuage.fillAmount = (float)(currentClearMoney / ClearMoney);

        CheckButtons();
    }

    public void CheckButtons()
    {
        tempAddBall_Price = addBall_BasePrice * (Current_Stage_Level) + (addBall_PriceScope * addBall_Level * (Current_Stage_Level + 1));
        tempMergeBalls_Price = mergeBalls_BasePrice * (Current_Stage_Level) + (mergeBalls_PriceScope * mergeBalls_Level * (Current_Stage_Level + 1));
        tempAddPin_Price = addPin_BasePrice * (Current_Stage_Level) + (addPin_PriceScope * addPin_Level * (Current_Stage_Level + 1));

        //tempAddBall_Price = addBall_BasePrice[Current_Stage_Level % addBall_BasePrice.Length] + addBall_Level * StageScope * (Current_Stage_Level + 1);
        //tempMergeBalls_Price = mergeBalls_BasePrice[Current_Stage_Level % mergeBalls_BasePrice.Length] + mergeBalls_Level * StageScope * (Current_Stage_Level + 1);
        //tempAddPin_Price = addPin_BasePrice[Current_Stage_Level % addPin_BasePrice.Length] + addPin_Level * StageScope * (Current_Stage_Level + 1);


        // 버튼 텍스트 업데이트
        Managers._uiGameScene.AddBallText.text = $"${ToCurrencyString(tempAddBall_Price)}"; // /*_gridManager.addBall_BasePrice * addBall_Level*/)}"
        Managers._uiGameScene.MergeBallsText.text = $"${ToCurrencyString(tempMergeBalls_Price)}";
        Managers._uiGameScene.AddPinText.text = $"${ToCurrencyString(tempAddPin_Price)}";


        // 금액 비교후 버튼 활성화 결정

        Managers._uiGameScene.AddBall_Button.interactable
            = Money >= tempAddBall_Price
            //addBall_BasePrice[Current_Stage_Level % Max_Stage] + addBall_Level * StageScope * (Current_Stage_Level + 1)
            //_gridManager.addBall_BasePrice * addBall_Level
            ? true : false;

        Managers._uiGameScene.MergeBalls_Button.interactable
             = Money >= tempMergeBalls_Price
             //(Money >= mergeBalls_BasePrice[Current_Stage_Level % Max_Stage] + mergeBalls_Level * StageScope * (Current_Stage_Level + 1) /*_gridManager.mergeBalls_BasePrice * mergeBalls_Level*/)
             && (tempMergeBalls.Count >= 3) ? true : false;

        Managers._uiGameScene.AddPin_Button.interactable
        = Money >= tempAddPin_Price
        //(Money >= addPin_BasePrice[Current_Stage_Level % Max_Stage] + addPin_Level * StageScope * (Current_Stage_Level + 1))
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

            // clear gate delete. clear Money check
            currentClearMoney += _money;
            Managers._uiGameScene.GuageText.text = $"{ToCurrencyString(currentClearMoney)} / {ToCurrencyString(ClearMoney)}";
            Managers._uiGameScene.FillGuage.fillAmount = (float)(currentClearMoney / ClearMoney);
            if (currentClearMoney >= ClearMoney)
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

    public void ResetBalls()
    {
        _gridManager._shooter.Ball_Wait_List.Clear();

        foreach (Ball _ball in ballList)
        {
            _ball.transform.GetComponent<TrailRenderer>().Clear();
            _ball.gameObject.SetActive(false);
            _gridManager._shooter.Ball_Wait_List.Add(_ball.GetComponent<Rigidbody>());
        }
    }


}
