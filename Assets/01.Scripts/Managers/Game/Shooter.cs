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

    // Start is called before the first frame update
    void Start()
    {
        Ball_Queue = new Queue<Rigidbody>();

        StartCoroutine(Cor_Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rigidbody _ball = Instantiate(Resources.Load<GameObject>("Ball")).GetComponent<Rigidbody>();
            _ball.transform.position = transform.position + Vector3.up * 2f;
            _ball.velocity = Vector3.up * Random.Range(Force * 0.8f, Force * 1.2f);
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

                _rb.AddForce(Vector3.up * Power);
            }



            yield return _interval;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Col");
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Col Ball");
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.DOMove(transform.position, 0.2f);
            Ball_Queue.Enqueue(other.GetComponent<Rigidbody>());
        }
    }
}
