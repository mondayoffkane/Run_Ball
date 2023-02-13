using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Child : MonoBehaviour
{

    public float Speed = 10f;
    public float Sense = 0.01f;

    public Player Parent;

    public float Limit_distance = 2f;

    [SerializeField] Mesh[] NumberMeshs;

    public int Num = 1;
    [SerializeField] int MaxNum = 99;

    public List<Calc_Obj> CalcObj_List;

    public Transform[] Digit_Objs;

    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f);

        NumberMeshs = Managers.Game.NumberMeshs;
        Digit_Objs = new Transform[3];

        for (int i = 0; i < 3; i++)
        {
            Digit_Objs[i] = transform.GetChild(i);
        }
        CheckDigit();
    }

    public void Init(int _num = 1)
    {
        Num = _num;


        //CheckDigit();
    }



    void Update()
    {

        transform.position +=
               new Vector3(Managers.Game.joyStickController.Dir.x, 0f, Managers.Game.joyStickController.Dir.y)
               * Speed * Sense;


        //transform.position


    }


    public void Calc(Calc_Obj.Type _type, int _value)
    {
        switch (_type)
        {

            case Calc_Obj.Type.Plus:
                Num += _value;

                break;

            case Calc_Obj.Type.Minus:
                Num -= _value;
                if (Num < 1)
                {
                    Parent.RemoveChild(this.gameObject);
                    return;
                }
                break;

            case Calc_Obj.Type.Multipl:
                Num *= _value;
                break;

            case Calc_Obj.Type.Merge:

                break;

            case Calc_Obj.Type.Add:

                break;
            default:
                break;
        }
        CheckDigit();
    }

    public void CheckDigit()
    {
        foreach (Transform _trans in Digit_Objs)
        {
            _trans.gameObject.SetActive(false);
        }

        switch (Num)
        {
            case int n when Num > 0 && Num < 10:
                Digit_Objs[0].gameObject.SetActive(true);
                Digit_Objs[0].GetComponent<MeshFilter>().sharedMesh = NumberMeshs[Num];
                Digit_Objs[0].localPosition = Vector3.zero;

                break;

            case int n when Num >= 10 && Num < 100:
                Digit_Objs[0].gameObject.SetActive(true);
                Digit_Objs[0].GetComponent<MeshFilter>().sharedMesh = NumberMeshs[Num % 10];
                Digit_Objs[0].localPosition = new Vector3(-0.3f, 0f, 0f);

                Digit_Objs[1].gameObject.SetActive(true);
                Digit_Objs[1].GetComponent<MeshFilter>().sharedMesh = NumberMeshs[Num / 10];
                Digit_Objs[1].localPosition = new Vector3(0.3f, 0f, 0f);


                break;


            default:
                break;
        }
    }


}
