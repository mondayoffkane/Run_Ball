using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Shooter : MonoBehaviour
{

    public float Interval = 0.1f;
    float Power = 500f;


    [ShowInInspector]
    public List<Rigidbody> Ball_Wait_List;
    [SerializeField] GameObject Ball_Pref;

    void Start()
    {
        Ball_Wait_List = new List<Rigidbody>();

        StartCoroutine(Cor_Shoot());
        if (Ball_Pref == null)
            Ball_Pref = Resources.Load<GameObject>("Ball");

    }



    IEnumerator Cor_Shoot()
    {
        WaitForSeconds _interval = new WaitForSeconds(Interval);
        while (true)
        {
            if (Ball_Wait_List.Count > 0)
            {
                Rigidbody _rb = Ball_Wait_List[0].GetComponent<Rigidbody>();
                Ball_Wait_List.Remove(_rb);
            
                DOTween.Kill(_rb);
                _rb.gameObject.SetActive(true);
                _rb.transform.position = transform.position;// + Vector3.right * Random.Range(-0.5f, 0.5f);
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _rb.isKinematic = false;
                _rb.AddForce(transform.up * Power);
                _rb.GetComponent<TrailRenderer>().enabled = true;
                _rb.GetComponent<Collider>().isTrigger = false;
             
            }

            yield return _interval;
        }
    }

    public Ball AddBall()
    {
        if (Ball_Pref == null)
            Ball_Pref = Resources.Load<GameObject>("Ball");
        Rigidbody _ball = Managers.Pool.Pop(Ball_Pref, transform).GetComponent<Rigidbody>(); // Instantiate(Ball_Pref).GetComponent<Rigidbody>();
        _ball.transform.position = transform.position; //+ Vector3.up * 2f;
      
        _ball.gameObject.SetActive(false);

        return _ball.GetComponent<Ball>();
    }


    public void MergeShoot(Ball _ball)
    {
        _ball.GetComponent<Ball>().isReady = true;
    
        _ball.transform.position = transform.position;
  
        Ball_Wait_List.Add(_ball.GetComponent<Rigidbody>());

    }

    
}
