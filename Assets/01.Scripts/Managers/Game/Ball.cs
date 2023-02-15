using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isReady = false;

    public double Price;
    public int index;
    public int Level;

   

    public void Init(int _index, int _level = 0, int _price = 1)
    {
        index = _index;
        Level = _level;
        Price = _price;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("isNotReady"))
        {
            isReady = false;
        }
    }

   
}
