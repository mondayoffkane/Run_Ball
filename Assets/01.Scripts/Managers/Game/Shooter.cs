using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Shooter : MonoBehaviour
{

    public float Interval = 0.1f;
    public float Power = 100f;
    public float Force = 20f;

    [ShowInInspector]
    public Queue<Rigidbody> Ball_Queue;
    [SerializeField] GameObject Ball_Pref;
    // Start is called before the first frame update
    void Start()
    {
        Ball_Queue = new Queue<Rigidbody>();

        StartCoroutine(Cor_Shoot());
        Ball_Pref = Resources.Load<GameObject>("Ball");

        //Managers.Pool.CreatePool(Ball_Pref);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddBall();
        }




    }

    IEnumerator Cor_Shoot()
    {
        WaitForSeconds _interval = new WaitForSeconds(Interval);
        while (true)
        {
            if (Ball_Queue.Count > 0)
            {
                Rigidbody _rb = Ball_Queue.Dequeue().GetComponent<Rigidbody>();
                if (_rb.GetComponent<Ball>().isReady == true)
                {
                    //_rb.GetComponent<Ball>().isReady = false;
                    DOTween.Kill(_rb);
                    _rb.velocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                    _rb.isKinematic = false;
                    _rb.AddForce(Vector3.up * Power);
                }
            }



            yield return _interval;
        }
    }

    public void AddBall()
    {
        Rigidbody _ball = Managers.Pool.Pop(Ball_Pref).GetComponent<Rigidbody>(); // Instantiate(Ball_Pref).GetComponent<Rigidbody>();
        _ball.transform.position = transform.position + Vector3.up * 2f;
        //_ball.velocity = Vector3.up * Force /*Random.Range(Force * 0.8f, Force * 1.2f)*/;
        _ball.AddForce(Vector3.up * Power);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.GetComponent<Ball>().isReady == false)
            {
                other.GetComponent<Ball>().isReady = true;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = transform.position;
                //other.transform.DOMove(transform.position, 0.1f);
                Ball_Queue.Enqueue(other.GetComponent<Rigidbody>());
            }
        }
    }
}
