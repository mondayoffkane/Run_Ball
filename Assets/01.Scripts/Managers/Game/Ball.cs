using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isReady = false;

    public double Price;

    public int Level;
    //public Material[] Mats;

    [SerializeField] MeshFilter _meshfilter;
    //[SerializeField] Renderer _renderer;
    //public Material _mat;

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



    public void Init(int _level = 0, int _basePrice = 5)
    {

        Level = _level;

        Price = Level == 0 ? 1 * (Managers.Game.Current_Stage_Level + 1)
            : _basePrice * Mathf.Pow(3, Level - 1) * (Managers.Game.Current_Stage_Level + 1);

        //_mat = Resources.Load<Material>("Material/" + Level) as Material;
        //if (_renderer == null) _renderer = GetComponent<Renderer>();
        //_renderer.sharedMaterial = Resources.Load<Material>("Material/" + Level) as Material;


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
        //else
        //{
        //    _rb.velocity *= 1.3f;
        //}


    }


}
