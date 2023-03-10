using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Current_Count++;
            if (Current_Count > Max_Count)
            {
                RockBreak();
            }
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
