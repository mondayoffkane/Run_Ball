using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pin : MonoBehaviour
{
    public Mesh[] Meshes;
    public PhysicMaterial _physicMat;

    MeshFilter _meshFilter;
    MeshCollider _meshCollider;

    public Transform Prev_Point;


    public enum PinType
    {
        Diamond,
        Spring,
        Rot


    }
    public PinType pinType;


    public double Price;

    // ===============================




    public void SetPin(PinType _pintype = PinType.Diamond)
    {
        switch (_pintype)
        {
            case PinType.Diamond:
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f));
                break;

            case PinType.Spring:

                break;

            case PinType.Rot:
                transform.DORotate(new Vector3(0f, 0f, 360f), 4f, RotateMode.FastBeyond360)
             .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);
                break;

            default:
                break;
        }

        pinType = _pintype;

        if (_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();
        if (_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();


        Mesh _mesh = Meshes[(int)pinType];
        _meshFilter.sharedMesh = _mesh;
        _meshCollider.sharedMesh = _mesh;
        _meshCollider.convex = true;
        _meshCollider.material = _physicMat;

    }




}
