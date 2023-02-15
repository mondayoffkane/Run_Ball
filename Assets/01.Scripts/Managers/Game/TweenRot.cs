using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRot : MonoBehaviour
{

    public float Rot_Speed = 360;
    public bool isRevers = false;

    public enum Rot_Dir
    {
        X,
        Y,
        Z
    }
    public Rot_Dir _Dir;
    [SerializeField] Vector3 Rot;

    Coroutine _cor;

    private void OnEnable()
    {
        if (_cor != null)
        {
            StopCoroutine(_cor);

            _cor = null;

        }
        _cor = StartCoroutine(Cor_Update());

    }




    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;
            float _value = isRevers == false ? Rot_Speed : -Rot_Speed;
            switch (_Dir)
            {
                case Rot_Dir.X:
                    Rot = new Vector3(_value, 0f, 0f);
                    break;

                case Rot_Dir.Y:
                    Rot = new Vector3(0f, _value, 0f);
                    break;

                case Rot_Dir.Z:
                    Rot = new Vector3(0f, 0f, _value);
                    break;
            }

            transform.Rotate(Rot * Time.deltaTime * Rot_Speed);
        }
    }


}
