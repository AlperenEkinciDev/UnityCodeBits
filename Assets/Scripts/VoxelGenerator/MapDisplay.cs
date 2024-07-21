using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapDisplay : MonoBehaviour
{
    Transform chunkTransform;
    MeshFilter VMeshFilter;
    MeshRenderer VMeshRenderer;
    public Material[] vMaterials;

    public void DrawVoxel(Transform chunkTransform, VoxelData voxelData)
    {
        VMeshFilter = chunkTransform.GetComponent<MeshFilter>();
        VMeshRenderer = chunkTransform.GetComponent<MeshRenderer>();

        VMeshFilter.sharedMesh = voxelData.CreateMesh();
        VMeshRenderer.sharedMaterials = vMaterials;

        VMeshRenderer.transform.GetComponent<MeshCollider>().sharedMesh = VMeshFilter.sharedMesh;
    }
}
