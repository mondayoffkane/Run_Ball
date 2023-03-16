using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    public int Max_Count = 100;
    public int Current_Count = 0;

    public float Scale = 2f;
    public Mesh[] _rockMeshes;
    public int num = -1;
    Mesh _mesh;
    MeshFilter _meshfilter;
    MeshCollider _meshcollider;

    public float _time = 0.2f;


    Text _countText;



    void Init()
    {
        if (_meshfilter == null)
            _meshfilter = GetComponent<MeshFilter>();
        if (_meshcollider == null)
            _meshcollider = GetComponent<MeshCollider>();
    }


    [Button]
    void SetRockMesh()
    {
        Init();
        if (num == -1)
        {
            _mesh = _rockMeshes[Random.Range(0, _rockMeshes.Length)];
        }
        else
        {
            _mesh = _rockMeshes[num];
        }

        _meshfilter.sharedMesh = _mesh;
        _meshcollider.sharedMesh = _mesh;
        transform.localScale = Vector3.one * Scale;
    }

    private void Start()
    {
        _countText = transform.GetComponentInChildren<Text>();
        _countText.text = $"{Max_Count - Current_Count}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
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


            Current_Count++;
            if (Current_Count > Max_Count)
            {
                Current_Count = Max_Count;
                RockBreak();
            }
            _countText.text = $"{Max_Count - Current_Count}";
        }
        // add col func
    }

    [Button]
    public void RockBreak()
    {
        transform.DOScale(Vector3.zero, _time)
            .OnComplete(() => gameObject.SetActive(false));
    }

}
