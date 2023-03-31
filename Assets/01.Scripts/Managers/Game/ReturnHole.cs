using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnHole : MonoBehaviour
{

    public GameObject Effect;


    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
          
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;           
            Managers.Game._currentShooter.Ball_Wait_List.Add(other.GetComponent<Rigidbody>());
            other.GetComponent<TrailRenderer>().Clear();
            other.GetComponent<TrailRenderer>().enabled = false;
            other.GetComponent<Collider>().isTrigger = true;
            other.gameObject.SetActive(false);

            StartCoroutine(Cor_Effect());
        }
    }


    IEnumerator Cor_Effect()
    {
        if (Effect != null)
        {
            Effect.SetActive(true);
            yield return new WaitForSeconds(1f);
            Effect.SetActive(false);
        }
    }
}
