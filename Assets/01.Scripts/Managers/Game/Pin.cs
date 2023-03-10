using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.ProBuilder.MeshOperations;

public class Pin : MonoBehaviour
{
    public Mesh[] Meshes;
    public PhysicMaterial _physicMat;

    MeshFilter _meshFilter;
    MeshCollider _meshCollider;

    public Transform Prev_Point;

    Text _text;

    bool isInit = false;

    public float ShootPower = 700f;
    public enum PinType
    {
        Diamond,
        Spring, // circle 
        Rot, // hex
        Triangle, // tri
        Cannon // 


    }
    public PinType pinType;
    public float Force = 10f;

    float _time = 0.2f;
    //Vector3 Start_Pos;
    // ===============================

    AudioClip _clip;
    public Mesh Temp_Mesh;
    public Material Temp_Mat;
    public GameObject Handle;

    Transform ChildObj;

    private void Awake()
    {
        // transform.GetChild(0).DOScaleZ(0.5f, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        //    Start_Pos = transform.position;
        _clip = Resources.Load<AudioClip>("Sound/Ball_1");
        if (Handle == null)
            Handle = transform.GetChild(1).gameObject;
        if (ChildObj == null) {
            ChildObj = transform.GetChild(0);
        }
        
    }



    public void SetPin(PinType _pintype = PinType.Diamond)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce)
            .OnComplete(() => isInit = true);

        //GetComponent<Renderer>().enabled = false;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActive(false);
        //}

        //transform.GetChild((int)_pintype).gameObject.SetActive(true);
        if (Handle == null) Handle = transform.GetChild(0).gameObject;
        Handle.SetActive(false);

        switch (_pintype)
        {
            case PinType.Diamond:
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 45f));

                break;

            case PinType.Spring:

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                break;

            case PinType.Rot:

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                transform.DORotate(new Vector3(0f, 0f, 360f), 4f, RotateMode.FastBeyond360)
              .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Restart);
                break;

            case PinType.Triangle:
                Handle.SetActive(true);
                break;

            case PinType.Cannon:
                Handle.SetActive(true);
                break;


            //case PinType.Fire:
            //    DOTween.Kill(transform);
            //    transform.localScale = Vector3.one;
            //    transform.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            //    break;

            default:
                break;
        }

        pinType = _pintype;

        if (_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();
        if (_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();


        Mesh _mesh = Meshes[(int)pinType];
        //_meshFilter.sharedMesh = _mesh;
        ChildObj.GetComponent<MeshFilter>().sharedMesh = _mesh;
        _meshCollider.sharedMesh = _mesh;
        _meshCollider.convex = true;
        _meshCollider.material = _physicMat;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            Ball _ball = collision.transform.GetComponent<Ball>();
            Transform _floating = Managers.Pool.Pop(Resources.Load<GameObject>("Floating_Money"), Managers.Game.transform).transform;
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

            Managers.Sound.Play(_clip);
        }
        if (isInit)
        {

            switch (pinType)
            {

                case PinType.Diamond:
                    DOTween.Kill(ChildObj);
                    DOTween.Sequence().Append(ChildObj.DOScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.Linear))
                        .Append(ChildObj.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.Linear));
                    break;


                case PinType.Spring:
                    DOTween.Kill(ChildObj);
                    Rigidbody _rb = collision.transform.GetComponent<Rigidbody>();
                    _rb.velocity = new Vector3(_rb.velocity.x, Force, 0f);


                    //transform.GetChild(0).DOScaleZ(0.5f, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
                    DOTween.Sequence().Append(ChildObj.DOScaleZ(0.5f, 0.1f).SetEase(Ease.Linear))
                        .Append(ChildObj.DOScaleZ(1f, 0.1f).SetEase(Ease.Linear));
                    break;

                case PinType.Rot:

                    DOTween.Sequence().Append(ChildObj.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.Linear))
                        .Append(ChildObj.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.Linear));
                    break;

                case PinType.Triangle:
                    DOTween.Kill(ChildObj);
                    DOTween.Sequence().Append(ChildObj.DOScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.Linear))
                        .Append(ChildObj.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.Linear));
                    break;

                case PinType.Cannon:
                    DOTween.Kill(ChildObj);
                    DOTween.Sequence().Append(ChildObj.DOScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.Linear))
                        .Append(ChildObj.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.Linear));

                    collision.transform.position = transform.position + transform.up;
                    collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * ShootPower);
                    break;


                //case PinType.Fire:

                //    collision.transform.GetComponent<MeshFilter>().mesh = Temp_Mesh;
                //    collision.transform.GetComponent<Renderer>().material = Temp_Mat;

                //    Rigidbody _rb2 = collision.transform.GetComponent<Rigidbody>();
                //    _rb2.velocity = new Vector3(Random.Range(-1f, 1f) * Force * 0.5f, Random.Range(0.5f, 1.5f) * Force, 0f);


                //    //transform.GetChild(0).DOScaleZ(0.5f, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
                //    DOTween.Sequence().Append(transform.DOScaleZ(0.5f, 0.1f).SetEase(Ease.Linear))
                //        .Append(transform.DOScaleZ(1f, 0.1f).SetEase(Ease.Linear));
                //    break;

                default:
                    //transform.DOShakePosition(_time, 0.2f); // 진


                    break;
            }
        }
    }







}
