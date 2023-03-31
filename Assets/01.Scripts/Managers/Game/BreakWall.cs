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
            _obj.GetComponent<Rigidbody>().AddExplosionForce(Power, transform.position, _Radius);
            _obj.transform.DOScale(Vector3.zero, _time);

        }
      
        GetComponent<Collider>().isTrigger = true;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (CanBreak)
        {
            Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Damage"), Managers.Game.transform).transform;
            _floating.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -1);

            Text _floatingText;
            _floatingText = _floating.transform.GetComponentInChildren<Text>();
            _floatingText.text = $"-{collision.transform.GetComponent<Ball>().Level + 1}";

            _floatingText.color = new Vector4(
                    _floatingText.color.r
                    , _floatingText.color.g
                    , _floatingText.color.b
                    , 1f);

            DOTween.Sequence().Append(_floating.DOMoveY(_floating.position.y + 1f, 0.5f).SetEase(Ease.Linear))
                .Join(_floatingText.DOColor(new Vector4(
                    _floatingText.color.r
                    , _floatingText.color.g
                    , _floatingText.color.b
                    , 0f), 0.5f)).SetEase(Ease.Linear)
                    .OnComplete(() => Managers.Pool.Push(_floating.GetComponent<Poolable>()));
            CollBall();
        }
    }


    public void CollBall()
    {
        Current_Count++;
        Count_Text.text = $"{Break_Count - Current_Count}";
        foreach (Transform _obj in _Pices)
        {
            _obj.transform.localScale = Vector3.one * (1f - 0.1f * ((float)Current_Count / (float)Break_Count));
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
