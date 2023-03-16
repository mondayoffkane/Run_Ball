using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MeshChanger : MonoBehaviour
{

    public Mesh[] meshList;


    MeshFilter _meshFilter;


    [Button]
    public void MeshChange(int _num=-1)
    {
        _meshFilter = GetComponent<MeshFilter>();

        if (_num == -1)
        {
            _meshFilter.sharedMesh = meshList[Random.Range(0, meshList.Length)];
        }
        else
        {
            _meshFilter.sharedMesh = meshList[_num];
        }
    }

}
