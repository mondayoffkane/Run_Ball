using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GridManager : MonoBehaviour
{
    public class LimitSize
    {
        public float Top = 13;
        public float Bottom = -7;
        public float Left = -6;
        public float Right = 6;
    }
    [ShowInInspector]
    public LimitSize limitSize;
    public float Offset_X = 3f, Offset_Y = 3f;
    int X, Y;

    public GameObject Pin_Pref;

    public GameObject Point;
    public Shooter _shooter;

    // ======================================
    public double addBall_BasePrice = 20;
    public double mergeBalls_BasePrice = 80;
    public double addPin_BasePrice = 100;



    private void Start()
    {
        Managers.Game._gridManager = this;
        if (_shooter == null)
        {
            _shooter = transform.GetComponentInChildren<Shooter>();
        }
        Managers.Game._currentShooter = _shooter;
        Managers.Game.MoneyUpdate();
        Managers.Game.StartStage();
        Pin_Pref = Resources.Load<GameObject>("Pin");
    }

    [Button]
    public void SetGrid()
    {

        int _count = transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        X = (int)((limitSize.Right - limitSize.Left) / Offset_X);
        Y = (int)((limitSize.Top - limitSize.Bottom) / Offset_Y);


        for (int i = 0; i < Y; i++)
        {
            for (int j = 0; j < X; j++)
            {
                Transform _point = Instantiate(Point).transform;
                _point.SetParent(transform);
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
                int _count = transform.childCount - 1;

                for (int i = 0; i < _count; i++)
                {
                    int _rnd = Random.Range(0, _count);
                    if (transform.GetChild(_rnd).GetComponent<Point>().Fix_Pin == null)
                    {
                        return transform.GetChild(_rnd).GetComponent<Point>();
                    }
                }

                for (int i = 0; i < _count; i++)
                {
                    if (transform.GetChild(i).GetComponent<Point>().Fix_Pin == null)
                    {
                        return transform.GetChild(i).GetComponent<Point>();
                    }
                }


                break;

            default:


                break;
        }

        return null;
    }



}
