using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stone : MonoBehaviour
{

    public float _time = 0.2f;


    public Vector3 Start_pos;

    private void Start()
    {
        Start_pos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))

            transform.DOShakePosition(_time,0.5f)
            .OnComplete(() => transform.position = Start_pos);

    }


}
