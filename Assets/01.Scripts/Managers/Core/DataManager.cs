public class DataManager
{
    ///<summary>Manager생산할때 만들어짐</summary>
    public void Init()
    {
        GetData();
    }


    public bool UseHaptic
    {
        get => _useHaptic;
        set
        {
            _useHaptic = value;
            ES3.Save<bool>("Haptic", value);
        }
    }
    private bool _useHaptic;

    public bool UseSound
    {
        get => _useSound;
        set
        {
            _useSound = value;
            ES3.Save<bool>("Sound", value);
        }
    }
    private bool _useSound;


    public void SaveData()
    {
    }
    public void GetData()
    {
        UseHaptic = ES3.Load<bool>("Haptic", true);
        UseSound = ES3.Load<bool>("Sound", true);
    }
}
