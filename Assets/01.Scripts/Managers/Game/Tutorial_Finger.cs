using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial_Finger : MonoBehaviour
{


    //public Vector3 Start_Pos, End_Pos;
    public float Move_interval = 1f;


    public Transform endTrans;

    public enum State
    {
        Move,
        Click

    }
    public State FingerState;

    public Ease _ease = Ease.Linear;


    void Start()
    {
        //Start_Pos = transform.position;

        switch (FingerState)
        {
            case State.Move:
                transform.DOMove(endTrans.position, Move_interval).SetEase(_ease).SetLoops(-1, LoopType.Restart);
                break;

            case State.Click:
                transform.DOScale(0.7f, 1f).SetEase(_ease).SetLoops(-1, LoopType.Yoyo);
                break;

            default:

                break;
        }
    }


}
