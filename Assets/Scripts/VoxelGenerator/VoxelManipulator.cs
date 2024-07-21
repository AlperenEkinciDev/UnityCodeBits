using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelManipulator : MonoBehaviour
{
    [Header("Voxel Manipulator Communication")]
    [SerializeField] CustomInputManager customInputManager;

    int brushSize = 3;
    RaycastHit hit;

    private void Start()
    {
        StartCoroutine(InputCheck());
    }

    IEnumerator InputCheck()
    {
        while (true)
        {
            if (customInputManager.GetCustomInputValue("Interact") > 0.9f)
            {
                LayerMask layerMask = LayerMask.GetMask("Terrain");

                if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 16, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.parent.CompareTag("Terrain"))
                    {
                        MapGenerator mapGenerator = hit.transform.parent.GetComponent<MapGenerator>();
                        MapDisplay mapDisplay = hit.transform.parent.GetComponent<MapDisplay>();

                        int CenterChunkNum = int.Parse(hit.collider.name.Split(' ')[1]);
                        int[] chunkArray = new int[9];


                        for (int i = 0; i < 9; i++)
                        {
                            chunkArray[i] = CenterChunkNum - (mapGenerator.densityMapWidth / mapGenerator.chunkSize + 1) + (i / 3) * (mapGenerator.densityMapWidth / mapGenerator.chunkSize) + i % 3;
                        }
                        for (int i = 0; i < 9; i++)
                        {
                            mapDisplay.DrawVoxel(mapGenerator.chunkTransforms[chunkArray[i]], VoxelGenerator.GenerateTerrainVoxel(VoxelGenerator.ManipulateVoxelAtPosition(hit, mapGenerator.noiseMap3D, mapGenerator.voxelObjectSize, mapGenerator.detailMultiplier, brushSize, 0.0f), mapGenerator.voxelObjectSize / mapGenerator.detailMultiplier, mapGenerator.voxelThreshold, mapGenerator.chunkSize * mapGenerator.detailMultiplier, chunkArray[i]));
                        }
                    }
                }
            }
            else if (customInputManager.GetCustomInputValue("Modify") > 0.9f)
            {
                LayerMask layerMask = LayerMask.GetMask("Terrain");

                if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 16, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.parent.CompareTag("Terrain"))
                    {
                        MapGenerator mapGenerator = hit.transform.parent.GetComponent<MapGenerator>();
                        MapDisplay mapDisplay = hit.transform.parent.GetComponent<MapDisplay>();

                        int CenterChunkNum = int.Parse(hit.collider.name.Split(' ')[1]);
                        int[] chunkArray = new int[9];


                        for (int i = 0; i < 9; i++)
                        {
                            chunkArray[i] = CenterChunkNum - (mapGenerator.densityMapWidth / mapGenerator.chunkSize + 1) + (i / 3) * (mapGenerator.densityMapWidth / mapGenerator.chunkSize) + i % 3;
                        }
                        for (int i = 0; i < 9; i++)
                        {
                            mapDisplay.DrawVoxel(mapGenerator.chunkTransforms[chunkArray[i]], VoxelGenerator.GenerateTerrainVoxel(VoxelGenerator.ManipulateVoxelAtPosition(hit, mapGenerator.noiseMap3D, mapGenerator.voxelObjectSize, mapGenerator.detailMultiplier, brushSize, 1.0f), mapGenerator.voxelObjectSize / mapGenerator.detailMultiplier, mapGenerator.voxelThreshold, mapGenerator.chunkSize * mapGenerator.detailMultiplier, chunkArray[i]));
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
