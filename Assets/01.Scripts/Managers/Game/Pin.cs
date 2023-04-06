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
        Stick,
        Triangle, // tri
        Square,
        Hex, // 
        Circle, //
        Cannon, //
        Trampoline


    }
    public PinType pinType;
    public float Force = 10f;

    //float _time = 0.2f;
    //Vector3 Start_Pos;
    // ===============================

    AudioClip _clip;
    public Mesh Temp_Mesh;
    public Material Temp_Mat;
    public GameObject Handle;

    Transform ChildObj;

    private void Awake()
    {
        
        _clip = Resources.Load<AudioClip>("Sound/Ball_1");
        if (Handle == null)
            Handle = transform.GetChild(1).gameObject;
        if (ChildObj == null)
        {
            ChildObj = transform.GetChild(0);
        }

    }



    public void SetPin(PinType _pintype = PinType.Triangle)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce)
            .OnComplete(() => isInit = true);

        if (Handle == null) Handle = transform.GetChild(0).gameObject;
        Handle.SetActive(false);

        switch (_pintype)
        {
            case PinType.Stick:
                Handle.SetActive(true);
                break;

            case PinType.Triangle:
                Handle.SetActive(true);
                break;

            case PinType.Cannon:
                Handle.SetActive(true);
                break;

            default:
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

                break;
        }

        pinType = _pintype;

        if (_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();
        if (_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();


        Mesh _mesh = Meshes[((int)pinType) * 3 + Random.Range(0, 3)];
    
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
           
            Managers.Game.FloatingTextFunc(_ball.Price, transform);

            Managers.Game.Vibe();
            Managers.Sound.Play(_clip);
        }
        if (isInit)
        {
            DOTween.Kill(ChildObj);
            DOTween.Sequence().Append(ChildObj.DOScale(Vector3.one * 0.5f, 0.1f).SetEase(Ease.Linear))
                .Append(ChildObj.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.Linear));
            switch (pinType)
            {

                case PinType.Cannon:
                   
                    collision.transform.position = transform.position + transform.up;
                    collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * ShootPower);
                    break;


                case PinType.Trampoline:

                    Rigidbody _rb = collision.transform.GetComponent<Rigidbody>();
                    _rb.velocity = new Vector3(_rb.velocity.x, Force, 0f);

                    break;


                default:
                   

                    break;
            }
        }
    }







}
