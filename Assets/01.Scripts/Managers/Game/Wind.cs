using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float addForce = 5f;
    Rigidbody _rb;

    public float reduece_scope = 0.8f;

    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }


    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.CompareTag("Ball"))
    //    {
    //        _rb = other.GetComponent<Rigidbody>();
    //        other.GetComponent<Rigidbody>().velocity = other.GetComponent<Rigidbody>().velocity.normalized;

    //        if (_rb.velocity.y < 0)
    //        {
    //            _rb.velocity = new Vector3(_rb.velocity.x, 0f, 0f);
    //        }



    //    }
    //}


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody _rb2 = other.GetComponent<Rigidbody>();


            float _dis = Vector3.Distance(transform.position, other.transform.position);
            if (_rb2.velocity.y < 0)
            {
                _rb2.velocity = new Vector3(_rb2.velocity.x, _rb2.velocity.y * reduece_scope, 0f);
            }
            if (_dis <= transform.lossyScale.y)
            {
                _rb2.AddForce(transform.up * addForce * (1 - _dis / transform.lossyScale.y));

            }
            if (_dis <= 0.8f)
            {
                _rb2.velocity = new Vector3(_rb2.velocity.x, addForce*0.2f, 0f);
                
            }
        }
    }
}
