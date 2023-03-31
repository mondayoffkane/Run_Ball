using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isReady = false;

    public double Price;

    public int Level;
  

    [SerializeField] MeshFilter _meshfilter;   

    [SerializeField] Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }
    private void Start()
    {
        //_renderer = GetComponent<Renderer>();
        _meshfilter = GetComponent<MeshFilter>();

    }



    public void Init(int _level = 0, float _scope = 4)
    {

        Level = _level;


        Price = (Managers.Game.ballBasePrice + Managers.Game.ball_PriceScope * (double)Managers.Game.Current_Stage_Level) * Mathf.Pow(_scope, Level);

      

        if (_meshfilter == null) _meshfilter = GetComponent<MeshFilter>();
        _meshfilter.sharedMesh = Resources.Load<Mesh>("BallMeshes/" + Level) as Mesh;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("isNotReady"))
        {
            isReady = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        _rb.AddTorque(Vector3.one * Random.Range(-360f, 360f));

        if (!collision.transform.CompareTag("NotJump"))
        {
            if (_rb.velocity.magnitude < 13)
            {
                _rb.velocity = _rb.velocity.normalized * 13f;
            }
        }

        float _X = _rb.velocity.x;

        switch (_X)
        {

            case float n when n > -0.2f && n < 0:
                _X = -0.5f;

                break;
            case float n when n >= 0f && n < 0.2:
                _X = 0.5f;
                break;
            default:
                break;
        }


        _rb.velocity = new Vector3(_X, _rb.velocity.y, 0f);

    }


}
