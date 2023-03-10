using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.UI;

public class BreakWall : MonoBehaviour
{
    public bool CanBreak = false;
    public int Break_Count = 100;
    public int Current_Count = 0;

    public float Power = 200
        ;

    public Transform[] _Pices;
    public float _Radius = 4f;
    public float _time = 1f;
    public Vector3 _offset = Vector3.up * 2;

    public Material[] Mat;

    public Text Count_Text;


    private void Start()
    {
        _Pices = new Transform[transform.childCount - 1];

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            _Pices[i] = transform.GetChild(i);
            _Pices[i].GetComponent<Renderer>().sharedMaterial = CanBreak ? Mat[0] : Mat[1];
        }

        if (Count_Text == null)
        {
            //Count_Text = transform.GetChild(transform.childCount - 1).GetComponent<Text>();
            Count_Text = transform.GetComponentInChildren<Text>();
        }

        Count_Text.gameObject.SetActive(CanBreak);


    }

    [Button]
    public void Explosion()
    {
        Count_Text.gameObject.SetActive(false);
        foreach (Transform _obj in _Pices)
        {
            _obj.GetComponent<Rigidbody>().isKinematic = false;
            //_obj.GetComponent<Rigidbody>().AddForce(Vector3.one * Random.Range(-1, 1f) * Power);
            _obj.GetComponent<Rigidbody>().AddExplosionForce(Power, transform.position, _Radius);
            _obj.transform.DOScale(Vector3.zero, _time);

        }
        //Collider[] _cols = Physics.OverlapSphere(transform.position, _Radius);

        //foreach (Collider _col in _cols)
        //{
        //    _col.GetComponent<Rigidbody>().isKinematic = false;
        //    _col.GetComponent<Rigidbody>().AddExplosionForce(Power, transform.position, _Radius);
        //}


        GetComponent<Collider>().isTrigger = true;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (CanBreak)
        {
            CollBall();
        }
    }


    public void CollBall()
    {
        //DOTween.Kill(transform);
        //DOTween.Sequence().Append(transform.DOScale(Vector3.one * 0.95f, 0.1f).SetEase(Ease.Linear))
        //              .Append(transform.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.Linear));

        Current_Count++;
        Count_Text.text = $"{Break_Count - Current_Count}";
        foreach (Transform _obj in _Pices)
        {
            _obj.transform.localScale = Vector3.one * (1f - 0.1f * (Current_Count / Break_Count));
        }
        if (Current_Count > 100)
        {
            Current_Count = Break_Count;
            Explosion();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Vector4(0.5f, 0.5f, 0.5f, 0.7f);
        Gizmos.DrawSphere(transform.position + _offset, _Radius);
    }



}
