using UnityEngine;
using UnityEngine.EventSystems;
public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Init();
        Managers.Scene.CurrentScene = this;

        //EventSystem
        EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
        if (eventSystem == null)
            Managers.Resource.Instantiate("EventSystem");
        else
        {
            eventSystem.gameObject.SetActive(true);
            eventSystem.enabled = true;
        }
    }

    protected abstract void Init();

    public abstract void Clear();
}
