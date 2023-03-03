using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkinMeshBaker : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer _skinmeshrenderer;
    [SerializeField] MeshCollider _meshCollider;
    [SerializeField] Mesh Temp_Mesh;


    void Start()
    {
        _skinmeshrenderer = GetComponent<SkinnedMeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        Temp_Mesh = new Mesh();

        BakeColliderMesh();
    }

    [Button]
    void BakeColliderMesh()
    {
        _skinmeshrenderer.BakeMesh(Temp_Mesh);
        _meshCollider.sharedMesh = Temp_Mesh;
    }

}
