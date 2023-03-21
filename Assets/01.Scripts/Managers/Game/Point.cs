using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    public Transform Fix_Pin;

    
    public void Reset_Pin()
    {
        Fix_Pin = null;
        GetComponent<Renderer>().enabled = true;

    }

    public void Set_Pin(Transform _newPin)
    {
        Fix_Pin = _newPin;
        GetComponent<Renderer>().enabled = false;   
    }

}
