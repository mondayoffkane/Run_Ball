using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Shooter : MonoBehaviour
{

    public float Interval = 0.1f;
    public float Power = 100f;


    [ShowInInspector]
    public Queue<Rigidbody> Ball_Queue;
    [SerializeField] GameObject Ball_Pref;

    void Start()
    {
        Ball_Queue = new Queue<Rigidbody>();

        StartCoroutine(Cor_Shoot());
        if (Ball_Pref == null)
            Ball_Pref = Resources.Load<GameObject>("Ball");

        //Managers.Pool.CreatePool(Ball_Pref);

    }



    IEnumerator Cor_Shoot()
    {
        WaitForSeconds _interval = new WaitForSeconds(Interval);
        while (true)
        {
            if (Ball_Queue.Count > 0)
            {
                Rigidbody _rb = Ball_Queue.Dequeue().GetComponent<Rigidbody>();
                //if (_rb.GetComponent<Ball>().isReady == true)
                //{
                DOTween.Kill(_rb);
                _rb.transform.position = transform.position + Vector3.right * Random.Range(-0.5f, 0.5f);
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _rb.isKinematic = false;
                _rb.AddForce(transform.up * Power);
                _rb.GetComponent<TrailRenderer>().enabled = true;
                _rb.GetComponent<Collider>().isTrigger = false;
                //}
            }



            yield return _interval;
        }
    }

    public Ball AddBall()
    {
        if (Ball_Pref == null)
            Ball_Pref = Resources.Load<GameObject>("Ball");
        Rigidbody _ball = Managers.Pool.Pop(Ball_Pref, transform).GetComponent<Rigidbody>(); // Instantiate(Ball_Pref).GetComponent<Rigidbody>();
        _ball.transform.position = transform.position + Vector3.up * 2f;
        //_ball.velocity = Vector3.up * Force /*Random.Range(Force * 0.8f, Force * 1.2f)*/;
        _ball.velocity = Vector3.zero;
        _ball.AddForce(Vector3.up * Power);


        return _ball.GetComponent<Ball>();
    }


    public void MergeShoot(Ball _ball)
    {
        _ball.GetComponent<Ball>().isReady = true;
        _ball.GetComponent<Rigidbody>().isKinematic = true;
        _ball.transform.position = transform.position;
        Ball_Queue.Enqueue(_ball.GetComponent<Rigidbody>());

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Ball"))
    //    {
    //        if (other.GetComponent<Ball>().isReady == false)
    //        {
    //            other.GetComponent<Ball>().isReady = true;
    //            other.GetComponent<Rigidbody>().isKinematic = true;
    //            other.transform.position = transform.position;
    //            //other.transform.DOMove(transform.position, 0.1f);
    //            Ball_Queue.Enqueue(other.GetComponent<Rigidbody>());
    //        }
    //    }
    //}
}
