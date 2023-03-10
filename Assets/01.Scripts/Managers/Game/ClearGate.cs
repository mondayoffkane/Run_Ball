using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ClearGate : MonoBehaviour
{

    public int max_Count = 100;
    public int current_Count = 0;

    public bool isOpen = false;

    public float _time = 2f;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            if (isOpen == false)
            {
                current_Count++;
                if (current_Count > max_Count)
                {
                    current_Count = max_Count;

                    StartCoroutine(OpenGate());
                }
            }
        }
    }








    [Button]
    IEnumerator OpenGate()
    {
        yield return null;
        isOpen = true;

        // add dotween visual direction
        transform.DOShakeRotation(1f);

        yield return new WaitForSeconds(_time);
        Managers.Game.StageClear();

    }


}
