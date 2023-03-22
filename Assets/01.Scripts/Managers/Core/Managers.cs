using UnityEngine;
public class Managers : MonoBehaviour
{
    ///<summary>내부적으로 사용되는 Managers 변수</summary>
    static Managers _instance;
    ///<summary>내부적으로 사용되는 Managers Property</summary>
    static Managers Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    GameManager _game;

    public static DataManager Data => Instance._data;
    public static PoolManager Pool => Instance._pool;
    public static ResourceManager Resource => Instance._resource;
    public static SceneManagerEx Scene => Instance._scene;
    public static SoundManager Sound => Instance._sound;
    public static UIManager UI => Instance._ui;

    public static UI_GameScene _uiGameScene;
    public static GameManager Game => Instance._game;



    ///<summary>가장 처음 매니저 만들때 한번 Init</summary>
    static void Init()
    {
        if (_instance == null)
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            UnityEngine.Input.multiTouchEnabled = false;

            GameObject go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();

            GameObject SceneTrasition = Managers.Resource.Instantiate("SceneTrasition");
            SceneTrasition.transform.SetParent(_instance.transform);
            Scene._sceneTrasitionAni = SceneTrasition.GetComponent<Animator>();

            _instance._data.Init();
            _instance._pool.Init();
            _instance._sound.Init();
            _instance._scene.Init();
            _instance._resource.Init();

            _instance._game = go.AddComponent<GameManager>();
        }
    }

    public static void GameInit()
    {
        _instance._game.Init();
    }

    ///<summary>새로운 씬으로 갈때마다 클리어</summary>
    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
        Resource.Clear();
        Game.Clear();
    }



    //============Check Internet============ 지워도 됨!
    //bool isInternetOn = true;
    //UI_PopupInternet popup;
    //float _oriTimeScale = 1;
    //private void Update()
    //{
    //    if (isInternetOn && Application.internetReachability == NetworkReachability.NotReachable)
    //    {
    //        isInternetOn = false;
    //        popup = Managers.UI.ShowPopupUI<UI_PopupInternet>();
    //        _oriTimeScale = Time.timeScale;
    //        Time.timeScale = 0;
    //    }
    //    else if (!isInternetOn && Application.internetReachability != NetworkReachability.NotReachable)
    //    {
    //        isInternetOn = true;
    //        if (popup != null)
    //            Managers.UI.ClosePopupUI(popup);
    //        Time.timeScale = _oriTimeScale;
    //    }
    //}
    //============Check Internet============ 지워도 됨!


    
}
