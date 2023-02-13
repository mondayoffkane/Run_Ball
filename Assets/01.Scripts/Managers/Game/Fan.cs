using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float Force = 5f;



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 _vec = other.GetComponent<Rigidbody>().velocity;
            other.GetComponent<Rigidbody>().velocity =
                new Vector3(Force, _vec.y, 0f);
        }
    }
}
