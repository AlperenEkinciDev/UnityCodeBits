using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelManipulator : MonoBehaviour
{
    [Header("Voxel Manipulator Communication")]
    [SerializeField] CustomInputManager customInputManager;
    [Header("Voxel Manipulator Settings")]
    [SerializeField] Transform indicatorTransform;
    [SerializeField][Range(3, 8)] int brushSize = 3;
    RaycastHit hit;

    private void Start()
    {
        StartCoroutine(InputCheck());
    }

    private void Update()
    {
        RaycastMouse();
        PlaceIndicator();
    }

    void PlaceIndicator()
    {
        if (hit.transform)
        {
            indicatorTransform.gameObject.SetActive(true);
            indicatorTransform.position = hit.point;
            indicatorTransform.localScale = Vector3.one * brushSize;
        }
        else
        {
            indicatorTransform.gameObject.SetActive(false);
        }
    }

    void RaycastMouse()
    {
        hit = new RaycastHit();

        LayerMask layerMask = LayerMask.GetMask("Terrain");
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore);
    }

    IEnumerator InputCheck()
    {
        while (true)
        {
            if (customInputManager.GetCustomInputValue("Modify") > 0.9f)
            {
                if (hit.transform)
                {
                    if (hit.transform.parent.CompareTag("Terrain"))
                    {
                        Draw3D(0.0f);
                    }
                }
            }
            else if (customInputManager.GetCustomInputValue("Interact") > 0.9f)
            {
                if (hit.transform)
                {
                    if (hit.transform.parent.CompareTag("Terrain"))
                    {
                        Draw3D(1.0f);
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Draw3D(float multiplier)
    {
        MapGenerator mapGenerator = hit.transform.parent.GetComponent<MapGenerator>();
        MapDisplay mapDisplay = hit.transform.parent.GetComponent<MapDisplay>();

        int CenterChunkNum = int.Parse(hit.collider.name.Split(' ')[1]);
        int[] chunkArray = new int[brushSize * brushSize];


        for (int i = 0; i < brushSize * brushSize; i++)
        {
            chunkArray[i] = CenterChunkNum - (mapGenerator.densityMapWidth / mapGenerator.chunkSize + 1) + (i / 3) * (mapGenerator.densityMapWidth / mapGenerator.chunkSize) + i % 3;
        }
        for (int i = 0; i < brushSize * brushSize; i++)
        {
            Transform chunkT = null;
            try
            {
                chunkT = mapGenerator.chunkTransforms[chunkArray[i]];
            }
            catch
            {
                continue;
            }
            mapDisplay.DrawVoxel(chunkT, VoxelGenerator.GenerateTerrainVoxel(VoxelGenerator.ManipulateVoxelAtPosition(hit, mapGenerator.noiseMap3D, mapGenerator.voxelObjectSize, mapGenerator.detailMultiplier, brushSize, multiplier), mapGenerator.voxelObjectSize / mapGenerator.detailMultiplier, mapGenerator.voxelThreshold, mapGenerator.chunkSize * mapGenerator.detailMultiplier, chunkArray[i]));
        }
    }
}
