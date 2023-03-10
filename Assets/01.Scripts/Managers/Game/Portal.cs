using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isInput = false;

    public Transform PairPortal;

    [SerializeField] float Power;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (isInput)
            {
                other.transform.position = PairPortal.transform.position;
                Power = other.GetComponent<Rigidbody>().velocity.magnitude;
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //other.GetComponent<Rigidbody>().AddForce(PairPortal.transform.up * Power);
                other.GetComponent<Rigidbody>().velocity = PairPortal.transform.up * Power;
            }
        }
    }
}
