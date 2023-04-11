using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;
//using UnityEditor.iOS;
using UnityEditor;
using Unity.VisualScripting;
using DG.Tweening;
using MondayOFF;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

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

    public bool isDoubleMoney;
    public float doubleMoney_Time = 30f;

    public double Mps = 0;

    double tempRewardMoney = 0d;
    double RV_RewardMoney = 0d;

    [SerializeField] double tempAddBall_Price, tempMergeBalls_Price, tempAddPin_Price;
    bool isMerging = false;
    Ball _newBall;
    Transform MergeRot;
    ParticleSystem MergeEffect;

    [SerializeField] int RV_Max_MergeLevel = 0;

    [SerializeField] bool NoAds = false;
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
        StartCoroutine(Cor_MPS_Update());
        MergeRot = GameObject.FindGameObjectWithTag("MergeRot").transform;
        MergeEffect = GameObject.FindGameObjectWithTag("MergeEffect").GetComponent<ParticleSystem>();
    }
    IEnumerator Cor_MPS_Update()
    {
        WaitForSeconds _interval = new WaitForSeconds(1f);
        Text _moneytime = Managers._uiGameScene.RV_DoubleMoney_Button.transform.GetChild(1).GetComponent<Text>();
        while (true)
        {
            yield return _interval;
            Managers._uiGameScene.MPSText.text = $"${ToCurrencyString(Mps)} / Sec";
            _moneytime.text = $"{doubleMoney_Time:0}s";
        }
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
        LoadData();

        SetStage(Current_Stage_Level);
    }

    public void SetStage(int _level = 0)
    {
        MondayOFF.EventTracker.TryStage(_level);
        if (_level == 1)
        {
            StartCoroutine(Cor_Review());
        }
        Managers._uiGameScene.StageText.text = $"Stage {Current_Stage_Level + 1}";

        if (Current_Stage != null)
        {
            Destroy(Current_Stage);
            Current_Stage = null;
        }
        Current_Stage = Instantiate(Resources.Load<GameObject>("Stages/Stage_" + _level % Max_Stage));
        _gridManager = Current_Stage.GetComponent<GridManager>();
        _currentShooter = _gridManager._shooter;

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


        IEnumerator Cor_Review()
        {
            yield return new WaitForSeconds(1f);
            CustomReviewManager.instance.StoreReview();
        }

    }

    public void StartStage()
    {
        Camera.main.transform.GetComponent<Skybox>().material =

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
            Money = 0;
            SaveData();
            SetStage(Current_Stage_Level);
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

            SaveData();
            SetStage(Current_Stage_Level);
        }



        if (Input.GetMouseButtonDown(0))
        {

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
                Vector3 _testpos = (mousePos - _rotObj.transform.position).normalized;

                //_rotObj.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.right - _testpos);
                _rotObj.rotation = Quaternion.FromToRotation(Vector3.down, _testpos);

            }

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
                        pinList.Remove(Pick_Obj.GetComponent<Pin>());
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
                        Temp_Next_Point.GetComponent<Point>().Set_Pin(Pick_Obj);
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

    #region "Base Upgrade"
    public void AddBall(bool isPlay = true)
    {
        if (isPlay == false)
        {
            addBallFunc();
        }
        else
        {
            if (Money >= tempAddBall_Price)
            {
                if (NoAds == false)
                {
                    AdsManager.ShowInterstitial();
                }

                Money -= tempAddBall_Price;
                addBall_Level++;
                addBallFunc();
            }
            else
            {
                if (NoAds == false)
                {
                    addBall_Level++;
                    AdsManager.ShowRewarded(() => addBallFunc());
                }
            }

        }
        /////////////////
        void addBallFunc()
        {
            Ball _newBall = _currentShooter.AddBall();
            _newBall.gameObject.SetActive(false);
            _currentShooter.Ball_Wait_List.Add(_newBall.GetComponent<Rigidbody>());
            ballList.Add(_newBall);
            _newBall.Init();

            CheckMergeList(); // 위치 변경 예정 , 돈 체크도 같이 해야함

            MoneyUpdate();
        }


    }

    public void MergeBalls()
    {

        if (Money >= tempMergeBalls_Price)
        {
            if (NoAds == false)
            {
                AdsManager.ShowInterstitial();
            }

            Money -= tempMergeBalls_Price;
            mergeBalls_Level++;
            mergeBallsFunc();
        }
        else
        {
            if (NoAds == false)
            {
                addBall_Level++;
                AdsManager.ShowRewarded(() => mergeBallsFunc());
            }
        }



        void mergeBallsFunc()
        {
            int tempLevel = 0;

            Button_OnOff(Managers._uiGameScene.MergeBalls_Button, false);
            isMerging = true;

            DOTween.Sequence().AppendCallback(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    tempMergeBalls[i].GetComponent<Rigidbody>().isKinematic = true;
                    tempMergeBalls[i].transform.SetParent(MergeRot);
                    tempMergeBalls[i].gameObject.SetActive(true);
                    tempMergeBalls[i].GetComponent<TrailRenderer>().Clear();

                }
            })
                .Append(tempMergeBalls[0].transform.DOMove(MergeRot.transform.GetChild(0).position, 0.5f)).SetEase(Ease.Linear)
                .Join(tempMergeBalls[1].transform.DOMove(MergeRot.transform.GetChild(1).position, 0.5f)).SetEase(Ease.Linear)
                .Join(tempMergeBalls[2].transform.DOMove(MergeRot.transform.GetChild(2).position, 0.5f)).SetEase(Ease.Linear)
                .Append(tempMergeBalls[0].transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .Join(tempMergeBalls[1].transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .Join(tempMergeBalls[2].transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    foreach (Ball _ball in tempMergeBalls)
                    {
                        tempLevel = _ball.Level;
                        Managers.Pool.Push(_ball.GetComponent<Poolable>());
                        ballList.Remove(_ball);
                        _currentShooter.Ball_Wait_List.Remove(_ball.GetComponent<Rigidbody>());
                    }
                    tempMergeBalls.Clear();

                    MergeEffect.Play();

                    _newBall = _currentShooter.AddBall();
                    _newBall.gameObject.SetActive(true);
                    _newBall.transform.position = MergeRot.transform.position;
                    _newBall.Init(tempLevel + 1);
                })
                .AppendInterval(0.5f)
                .OnComplete(() =>
                {

                    _newBall.gameObject.SetActive(false);
                    ballList.Add(_newBall);
                    _currentShooter.MergeShoot(_newBall);
                    CheckMergeList();

                    MoneyUpdate();
                    isMerging = false;
                    CheckButtons();

                    if ((tempLevel + 1 > RV_Max_MergeLevel))
                    {
                        RV_Max_MergeLevel = tempLevel + 1;
                        Managers._uiGameScene.Merge_RV_Panel.SetActive(true);
                        // add panel
                        foreach (RawImage _img in Managers._uiGameScene.Ball_Render_Imgs)
                        {
                            _img.enabled = false;
                        }
                        Managers._uiGameScene.Ball_Render_Imgs[RV_Max_MergeLevel].enabled = true;
                    }
                });
        }
    }

    public void AddPin(int _num = 0, bool isPay = true)
    {
        if (isPay == false)
        {
            Pin _pin = Managers.Pool.Pop(_gridManager.Pin_Pref, transform).GetComponent<Pin>();
            Point _point = _gridManager.Point_Group.GetChild(2).GetComponent<Point>();

            _point.Fix_Pin = _pin.transform;
            _pin.transform.position = _point.transform.position;
            _point.GetComponent<Renderer>().enabled = false;
            switch (_num)
            {
                case 0:
                    _pin.GetComponent<Pin>().SetPin(_gridManager
                        .PinType_Array[new System.Random().Next(0, _gridManager.PinType_Array.Length)]);
                    break;

                default:
                    break;
            }
            _pin.GetComponent<Pin>().Prev_Point = _point.transform;
            pinList.Add(_pin);
            MoneyUpdate();
            Managers.Sound.Play(Resources.Load<AudioClip>("Sound/Spawn"));
                        
        }
        else
        {
            if (Money >= tempAddPin_Price)
            {
                if (NoAds == false)
                {
                    AdsManager.ShowInterstitial();
                }
                Money -= tempAddPin_Price;
                addPin_Level++;
                addPinFunc();
            }
            else
            {
                if (NoAds == false)
                {
                    addPin_Level++;
                    AdsManager.ShowRewarded(() => addPinFunc());
                }
            }
        }

        void addPinFunc()
        {
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
    }

    #endregion

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

        tempRewardMoney = 0d;
        foreach (Ball _ball in ballList)
        {
            tempRewardMoney += _ball.Price;
        }


        RV_RewardMoney = tempRewardMoney * 50d;
        Managers._uiGameScene.RV_AddMoney_Button.GetComponentInChildren<Text>().text = $"{ToCurrencyString(RV_RewardMoney)}";

        CheckButtons();
    }

    public void CheckButtons()
    {
        tempAddBall_Price = addBall_BasePrice * (Current_Stage_Level) + (addBall_PriceScope * addBall_Level * (Current_Stage_Level + 1));
        tempMergeBalls_Price = mergeBalls_BasePrice * (Current_Stage_Level) + (mergeBalls_PriceScope * mergeBalls_Level * (Current_Stage_Level + 1));
        tempAddPin_Price = addPin_BasePrice * (Current_Stage_Level) + (addPin_PriceScope * addPin_Level * (Current_Stage_Level + 1));

        // 버튼 텍스트 업데이트
        Managers._uiGameScene.AddBallText.text = $"${ToCurrencyString(tempAddBall_Price)}"; // /*_gridManager.addBall_BasePrice * addBall_Level*/)}"
        Managers._uiGameScene.MergeBallsText.text = $"${ToCurrencyString(tempMergeBalls_Price)}";
        Managers._uiGameScene.AddPinText.text = $"${ToCurrencyString(tempAddPin_Price)}";


        // 금액 비교후 버튼 활성화 결정
        if (NoAds == true)
        {
            Button_OnOff(Managers._uiGameScene.AddBall_Button,
               (Money >= tempAddBall_Price));
            Button_OnOff(Managers._uiGameScene.MergeBalls_Button,
               (Money >= tempMergeBalls_Price && (tempMergeBalls.Count >= 3) && !isMerging));
            Button_OnOff(Managers._uiGameScene.AddPin_Button,
                (Money >= tempAddPin_Price && (_gridManager.FindEmptyPoint(GridManager.FindState.Random) != null)));
        }
        else
        {

            Button_OnOff(Managers._uiGameScene.AddBall_Button, true,
               (Money >= tempAddBall_Price));
            Button_OnOff(Managers._uiGameScene.MergeBalls_Button, true && (tempMergeBalls.Count >= 3) && !isMerging,
               (Money >= tempMergeBalls_Price));
            Button_OnOff(Managers._uiGameScene.AddPin_Button, true && (_gridManager.FindEmptyPoint(GridManager.FindState.Random) != null),
                (Money >= tempAddPin_Price));
        }



    }

    public void Button_OnOff(Button _button, bool isTrue, bool enoughMoney = true)
    {

        _button.interactable = isTrue;
        _button.transform.GetChild(0).GetComponent<Text>().color = isTrue ? new Color32(255, 255, 255, 255) : new Color32(175, 175, 175, 255);
        _button.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = enoughMoney;
        _button.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = !enoughMoney;
        _button.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = isTrue ? new Color32(255, 255, 255, 255) : new Color32(175, 175, 175, 255);
        _button.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = isTrue ? new Color32(255, 255, 255, 255) : new Color32(175, 175, 175, 255);
        _button.transform.GetChild(1).GetComponent<Outline>().enabled = isTrue;

        if (!enoughMoney)
        {
            _button.transform.GetChild(0).GetComponent<Text>().text = $"Free";
        }

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

        MondayOFF.EventTracker.ClearStage(Current_Stage_Level);

        isRunning = false;
        Current_Stage_Level++;

        Managers.Sound.Play(Resources.Load<AudioClip>("Sound/Clear"));

        Debug.Log("Stage Clear!");
        Managers._uiGameScene.Upgrade_Panel.SetActive(false);
        Managers._uiGameScene.Clear_Panel.SetActive(true);

        SaveData();

    }

    public void NextStage_Button()
    {
        MondayOFF.AdsManager.ShowInterstitial();
        isRunning = false;
        currentClearMoney = 0;

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


    public void FloatingTextFunc(double _price, Transform _trans)
    {
        double _PriceValue = isDoubleMoney ? _price * 2d : _price;


        Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Money"), Managers.Game.transform).transform;
        _floating.position = new Vector3(_trans.position.x, _trans.position.y, -1);

        Text _floatingText;
        _floatingText = _floating.transform.GetComponentInChildren<Text>();
        _floatingText.text = $"$ {ToCurrencyString(_PriceValue)}";

        _floatingText.color = new Vector4(
                _floatingText.color.r
                , _floatingText.color.g
                , _floatingText.color.b
                , 1f);

        DOTween.Sequence().Append(_floating.DOMoveY(_floating.position.y + 1f, 0.5f).SetEase(Ease.Linear))
            .Join(_floatingText.DOColor(new Vector4(
                _floatingText.color.r
                , _floatingText.color.g
                , _floatingText.color.b
                , 0f), 0.5f)).SetEase(Ease.Linear)
                .OnComplete(() => Managers.Pool.Push(_floating.GetComponent<Poolable>()));



        AddMoney(_PriceValue);

        DOTween.Sequence().AppendCallback(() => MPS_Update(_PriceValue))
            .AppendInterval(1f)
            .AppendCallback(() => MPS_Update(-_PriceValue));





    }

    public void MPS_Update(double _val)
    {
        Mps += _val;

    }


    /////////////// RV Func ////////////////////

    #region "RV Func"

    public void RV_AddMoney()
    {
        EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "Right_RV", "AddMoney" } });

        Money += RV_RewardMoney;
        MoneyUpdate();
    }

    public void RV_AddBall(int _count = 10)
    {
        EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "Right_RV", "AddBall" } });
        for (int i = 0; i < _count; i++)
        {
            AddBall(false);
        }
    }

    public void RV_DoubleMoney()
    {
        if (!isDoubleMoney)
        {
            DOTween.Sequence().AppendCallback(() =>
            {
                isDoubleMoney = true;
                DOTween.To(() => 30f, x => doubleMoney_Time = x, 0, 30f).SetEase(Ease.Linear);
                EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "Right_RV", "DoubleMoney" } });
                Managers._uiGameScene.RV_DoubleMoney_Button.interactable = false;
            })
                .AppendInterval(30f).
                AppendCallback(() =>
                {
                    isDoubleMoney = false;
                    doubleMoney_Time = 30f;
                    Managers._uiGameScene.RV_DoubleMoney_Button.interactable = true;
                });

        }
    }

    public void RV_Merge_Reward()
    {
        _newBall = _currentShooter.AddBall();
        _newBall.gameObject.SetActive(true);
        _newBall.transform.position = MergeRot.transform.position;
        _newBall.Init(RV_Max_MergeLevel);

        _newBall.gameObject.SetActive(false);
        ballList.Add(_newBall);
        _currentShooter.MergeShoot(_newBall);
        CheckMergeList();

        Managers._uiGameScene.Merge_RV_Panel.SetActive(false);
    }

    #endregion
    // /// ////////////////////////////


    public void GameObjectOnOnff(GameObject _obj)
    {
        _obj.SetActive(!_obj.activeSelf);
    }

    public void Vibe(int _num = 0)
    {
        if (Managers.Data.UseHaptic)
        {
            switch (_num)
            {
                case 0:
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                    break;

                case 1:
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                    break;

                case 2:
                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                    break;

                default:
                    break;
            }
        }
    }


    /// 4.10
    //void 

}