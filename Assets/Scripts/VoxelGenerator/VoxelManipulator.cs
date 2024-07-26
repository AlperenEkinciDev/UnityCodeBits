using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelManipulator : MonoBehaviour
{
    [Header("Voxel Manipulator Communication")]
    [SerializeField] MapGenerator mapGenerator;
    [Header("Voxel Manipulator Settings")]
    [SerializeField] Transform indicatorTransform;
    [SerializeField][Range(3, 8)] int brushSize = 3;
    [SerializeField] float movementThreshold = 1f;
    CustomInputManager customInputManager;
    AudioManager audioManager;
    RaycastHit hit;
    bool isFirstClick = true;
    bool isInGenerationProcess = false;

    private void Start()
    {
        customInputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<CustomInputManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        RaycastMouse();
        PlaceIndicator();
        CheckAndDraw();
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

    private void CheckAndDraw()
    {
        bool isModify = customInputManager.GetCustomInputValue("Modify") > 0.9f;
        bool isInteract = customInputManager.GetCustomInputValue("Interact") > 0.9f;
        bool isMoving = (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"))) > movementThreshold;

        if (hit.transform)
        {
            if (hit.transform.parent.CompareTag("Terrain"))
            {
                if (isModify && isMoving || isModify && isFirstClick) { Draw3D(0.0f); isFirstClick = false;}
                else if (isInteract && isMoving || isInteract && isFirstClick) { Draw3D(1.0f); isFirstClick = false;}
                else if (isMoving || !isModify && !isInteract) isFirstClick = true;
            }
        }

        bool isGenerating = customInputManager.GetCustomInputValue("Reset") > 0.9f;
        bool isRandom = customInputManager.GetCustomInputValue("Randomize") > 0.9f;

        if (isGenerating && !isInGenerationProcess)
        {
            isInGenerationProcess = true;
            if (isRandom)
            {
                int randSeed = Random.Range(1, 1000);
                mapGenerator.seed = randSeed;
            }
            mapGenerator.GenerateMap();
        }
        else if(!isGenerating)
        {
            isInGenerationProcess = false;
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

    public void IncrementBrushSize()
    {
        brushSize++;
        brushSize = Mathf.Clamp(brushSize, 3, 8);
    }
    public void DecrementBrushSize()
    {
        brushSize--;
        brushSize = Mathf.Clamp(brushSize, 3, 8);
    }
}
