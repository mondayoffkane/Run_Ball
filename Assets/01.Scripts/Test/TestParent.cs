using UnityEngine;
using UnityEngine.EventSystems;
public abstract class TestParent : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Parent");
        Init();
    }


    protected abstract void Init();
}
