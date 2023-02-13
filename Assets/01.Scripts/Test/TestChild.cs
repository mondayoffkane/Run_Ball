using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChild : TestParent
{
    //private void Awake()
    //{
    //    Debug.Log("Child_Test");
    //}

    protected override void Init()
    {

        Debug.Log("Child");

    }
}
