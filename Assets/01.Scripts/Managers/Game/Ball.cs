using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isReady = false;

    public double Price;

    public int Level;
    //public Material[] Mats;

    [SerializeField] Renderer _renderer;
    //public Material _mat;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }



    public void Init(int _level = 0, int _basePrice = 5)
    {

        Level = _level;

        Price = Level == 0 ? 1 * (Managers.Game.Current_Stage_Level + 1)
            : _basePrice * Mathf.Pow(3, Level - 1) * (Managers.Game.Current_Stage_Level + 1);

        //_mat = Resources.Load<Material>("Material/" + Level) as Material;
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        _renderer.sharedMaterial = Resources.Load<Material>("Material/" + Level) as Material;

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
        GetComponent<Rigidbody>().AddTorque(Vector3.forward * Random.Range(-360f, 360f));
    }


}
