using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public bool isInput = false;

    public Transform PairPortal;

    [SerializeField] float Power;

    public Mesh[] _cupMeshes;

    [SerializeField] Image[] Arrow_Img;

    private void Start()
    {
        GetComponent<MeshFilter>().sharedMesh = isInput ?
            _cupMeshes[0] : _cupMeshes[1];

        Arrow_Img = new Image[2];
        Arrow_Img = transform.GetComponentsInChildren<Image>();

        Arrow_Img[0].enabled = !isInput;
        Arrow_Img[1].enabled = isInput;


    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ball"))
        {
            if (isInput)
            {
                other.transform.position = PairPortal.transform.position;
                other.GetComponent<TrailRenderer>().Clear();
                Power = other.GetComponent<Rigidbody>().velocity.magnitude;
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                //other.GetComponent<Rigidbody>().AddForce(PairPortal.transform.up * Power);
                other.GetComponent<Rigidbody>().velocity = PairPortal.transform.up * Power;
            }
        }
    }
}
