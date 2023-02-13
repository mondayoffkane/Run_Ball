using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float Speed = 10f;
    public float Sense = 0.01f;

    public Transform Parent;
    public float Limit_distance = 5f;

    public bool isChild = false;

    private void Start()
    {

    }


    void Update()
    {
        //if (!isChild)
        //{
        //    transform.position +=
        //        new Vector3(Managers.Game.joyStickController.Dir.x, 0f, Managers.Game.joyStickController.Dir.y)
        //        * Speed * Sense;
        //}
        //else
        //{
        //    if (Vector3.Distance(transform.position, Parent.position) > Limit_distance)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, Parent.position, Time.deltaTime * Speed);
        //    }
        //}


        ////////  //////////////

        transform.position +=
               new Vector3(Managers.Game.joyStickController.Dir.x, 0f, Managers.Game.joyStickController.Dir.y)
               * Speed * Sense;
        //if (isChild)
        //{
        //    if (Vector3.Distance(transform.position, Parent.position) > Limit_distance)
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, Parent.position, Time.deltaTime * Speed);
        //    }
        //}


    }
}
