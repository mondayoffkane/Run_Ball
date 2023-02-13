using System.Collections;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    Coroutine timerCor;
    private bool _isUsing;
    public bool IsUsing
    {
        get { return _isUsing; }
        set
        {
            _isUsing = value;
            if (!_isUsing && timerCor != null)
            {
                StopCoroutine(timerCor);
            }
        }
    }
    ///<summary>잠시 후 오브젝트를 끄고 Pool로 Push하는 역할</summary>
    public void Timer(float timer)
    {
        timerCor = StartCoroutine(TimerCor(timer));
    }
    IEnumerator TimerCor(float timer)
    {
        yield return Util.WaitGet(timer);
        Managers.Resource.Destroy(this.gameObject);
    }
}