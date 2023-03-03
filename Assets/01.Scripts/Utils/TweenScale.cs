using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenScale : MonoBehaviour
{
    public enum State
    {
        Scale,
        Move
    }
    public State _state;


    public float _scale = 1.2f;
    public float _time = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        switch (_state)
        {
            case State.Scale:
                transform.DOScale(Vector3.one * _scale, _time).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

                break;

            case State.Move:
                transform.localPosition = new Vector3(330f, 400f, 0f);
                transform.DOLocalMoveX(-67f, 1.5f).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Restart);

                break;

            default:
                break;
        }
    }


}
