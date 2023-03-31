using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Ring : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball _ball = other.GetComponent<Ball>();           

            Managers.Game.FloatingTextFunc(_ball.Price, transform);

            

        }
    }
}
