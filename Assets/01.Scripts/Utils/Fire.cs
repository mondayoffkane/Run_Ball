using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Mesh Temp_Mesh;
    public Material Temp_Mat;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<MeshFilter>().mesh = Temp_Mesh;
            other.GetComponent<Renderer>().material = Temp_Mat;



        }
    }


}
