using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GridManager : MonoBehaviour
{
    public class LimitSize
    {
        public float Top = 8;
        public float Bottom = -6;
        public float Left = -5;
        public float Right = 5;
    }
    [ShowInInspector]
    public LimitSize limitSize;
    public int Width = 5, Height = 5;
    [SerializeField] float Offset_X = 3f, Offset_Y = 3f;
    //public float 

    public GameObject Pin_Pref;

    public GameObject Point;
    public Shooter _shooter;
    public Transform Point_Group;

    [ShowInInspector]
    public Pin.PinType[] PinType_Array = new Pin.PinType[1];

    // ======================================
    //public double addBall_BasePrice = 20;
    //public double mergeBalls_BasePrice = 80;
    //public double addPin_BasePrice = 100;
    //public double ClearMoney = 1000;
    //public Color BackGround_Color;

    // public Material SkyBox_Mat;


    [Space(10)]
    [Header("Test")]
    [ReadOnly]public string Buttons;

    private void Start()
    {
        //Managers.Game._gridManager = this;
        //if (_shooter == null)
        //{
        //    _shooter = transform.GetComponentInChildren<Shooter>();
        //}
        //Managers.Game._currentShooter = _shooter;
        Managers.Game.MoneyUpdate();
        //Managers.Game.StartStage();
        Pin_Pref = Resources.Load<GameObject>("Pin");
        // Camera.main.backgroundColor = BackGround_Color;
    }

    [Button]
    public void SetGrid()
    {

        int _count = Point_Group.childCount;
        for (int i = 0; i < _count; i++)
        {
            DestroyImmediate(Point_Group.GetChild(0).gameObject);
        }

        //X = Mathf.CeilToInt((limitSize.Right - limitSize.Left) / Offset_X);
        //Y = Mathf.CeilToInt((limitSize.Top - limitSize.Bottom) / Offset_Y);

        Offset_X = (limitSize.Right - limitSize.Left) / (float)(Width - 1);
        Offset_Y = (limitSize.Top - limitSize.Bottom) / (float)(Height - 1);


        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Transform _point = Instantiate(Point).transform;
                _point.SetParent(Point_Group);
                _point.position = new Vector3(limitSize.Left + j * Offset_X, limitSize.Bottom + i * Offset_Y, 0f);
            }
        }



    }

    public enum FindState
    {
        Bottom,
        Top,
        Random
    }

    public Point FindEmptyPoint(FindState _state)
    {
        switch (_state)
        {
            case FindState.Top:

                break;

            case FindState.Bottom:

                break;

            case FindState.Random:
                int _count = Point_Group.childCount;

                for (int i = 0; i < _count; i++)
                {
                    int _rnd = Random.Range(0, _count);
                    if (Point_Group.GetChild(_rnd).GetComponent<Point>().Fix_Pin == null)
                    {
                        return Point_Group.GetChild(_rnd).GetComponent<Point>();
                    }
                }

                for (int i = 0; i < _count; i++)
                {
                    if (Point_Group.GetChild(i).GetComponent<Point>().Fix_Pin == null)
                    {
                        return Point_Group.GetChild(i).GetComponent<Point>();
                    }
                }


                break;

            default:


                break;
        }

        return null;
    }



}
