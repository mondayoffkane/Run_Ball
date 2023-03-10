using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class Mover_Stick : MonoBehaviour
{

    public float _time = 3f;

    SkinnedMeshRenderer _skinnedmeshrend;
    MeshCollider _meshcollider;

    Mesh _mesh;

    [SerializeField] float _value;
    void Init()
    {
        if (_skinnedmeshrend == null)
            _skinnedmeshrend = GetComponent<SkinnedMeshRenderer>();
        if (_meshcollider == null)
            _meshcollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        _mesh = new Mesh();
        Init();
        DOTween.To(() => _value, x => _value = x, 100, _time).SetEase(Ease.Linear)
            .OnComplete(() => DOTween.To(() => _value, x => _value = x, 0, _time).SetEase(Ease.Linear))
            .SetRelative(true).SetLoops(-1);


    }


    private void Update()
    {
        _skinnedmeshrend.SetBlendShapeWeight(0, _value);
        BakeSkinnedMeshCol();
    }


    [Button]
    public void BakeSkinnedMeshCol()
    {
        Init();

        _skinnedmeshrend.BakeMesh(_mesh);
        _meshcollider.sharedMesh = _mesh;

    }


}
