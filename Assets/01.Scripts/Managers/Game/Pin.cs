using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Pin : MonoBehaviour
{
    public Mesh[] Meshes;
    public PhysicMaterial _physicMat;

    MeshFilter _meshFilter;
    MeshCollider _meshCollider;

    public Transform Prev_Point;

    Text _text;

    public enum PinType
    {
        Diamond,
        Spring,
        Rot


    }
    public PinType pinType;
    public float Force = 10f;

    float _time = 0.2f;
    //Vector3 Start_Pos;
    // ===============================


    //private void Start()
    //{
    //    Start_Pos = transform.position;
    //}



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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Ball _ball = collision.transform.GetComponent<Ball>();
            Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Money")).transform;
            _floating.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -1);



            Text _floatingText;
            _floatingText = _floating.transform.GetComponentInChildren<Text>();
            _floatingText.text = $"$ {_ball.Price}";

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

            //Managers.Game.Money += (double)_ball.Price;
            //Managers.Game.MoneyUpdate();
            Managers.Game.AddMoney(_ball.Price);
            //Managers.UI.
        }

        switch (pinType)
        {
            case PinType.Spring:
                Rigidbody _rb = collision.transform.GetComponent<Rigidbody>();
                _rb.velocity = new Vector3(_rb.velocity.x, Force, 0f);
                break;

            default:
                //transform.DOShakePosition(_time, 0.2f); // 진


                break;
        }
    }







}
