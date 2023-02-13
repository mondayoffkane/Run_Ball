using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc_Obj : MonoBehaviour
{

    public enum Type
    {
        Plus,
        Minus,
        Multipl,
        Merge,
        Add
    }
    public Type type;

    public int Value = 1;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Child"))
        {
            other.GetComponent<Child>().Calc(type, Value);
        }
    }
}
