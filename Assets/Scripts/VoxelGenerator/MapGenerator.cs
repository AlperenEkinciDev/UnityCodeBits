using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        Voxel
    }
    public DrawMode drawMode;

    public int densityMapWidth = 2;//Always make it divisible by chunkSize.Also same as densityMapDepth value
    public int densityMapHeight = 2;
    public int densityMapDepth = 2;//Always make it divisible by chunkSize.Also same as densityMapWidth value
    [Range(1, 8)]
    public int detailMultiplier;
    public float voxelThreshold = 0.1f;
    public float voxelObjectSize = 1f;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public bool useFalloff;
    public float fallOffMultiplier = 1.0f;

    public bool useDegredation;
    public float degredationRate = 0.0f;

    public bool useFillLayer;
    public int fillLayerLowLimit = 0;
    public int fillLayerHighLimit = 0;
    public float fillValue = 0;

    public bool autoUpdate;

    public TerrainType[] regions;

    [HideInInspector]public float[,,] noiseMap3D;

    
    public Transform[] chunkTransforms;
    public int chunkSize;//ONLY USE 4, 8 AND 16

    private void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        ResetChunkTransforms(densityMapWidth, densityMapHeight, densityMapDepth, chunkSize, voxelObjectSize);

        noiseMap3D = Noise.GenerateNoiseMap3D(densityMapWidth, densityMapHeight, densityMapDepth, detailMultiplier, seed, noiseScale, octaves, persistance, lacunarity, useFalloff, fallOffMultiplier, useDegredation, degredationRate, offset, fillLayerLowLimit, fillLayerHighLimit, fillValue, useFillLayer);
        
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.Voxel)
        {
            for (int i = 0; i < (densityMapWidth / chunkSize) * (densityMapDepth / chunkSize); i++)
            {
                display.DrawVoxel(chunkTransforms[i], VoxelGenerator.GenerateTerrainVoxel(noiseMap3D, voxelObjectSize / detailMultiplier, voxelThreshold, chunkSize * detailMultiplier, i));
            }
        }
    }

    void ResetChunkTransforms(int mapWidth, int mapHeight, int mapDepth, int chunkSize, float voxelObjectSize)
    {
        DeleteChildObjects();

        mapWidth = mapWidth / chunkSize;//**
        mapDepth = mapDepth / chunkSize;//**

        chunkTransforms = new Transform[mapWidth * mapDepth];

        for (int z = 0; z < mapDepth; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int chunkNum = z * mapWidth + x;

                GameObject chunkObject = new GameObject("Chunk " + chunkNum);
                chunkObject.transform.parent = this.transform;
                chunkObject.layer = chunkObject.transform.parent.gameObject.layer;
                chunkObject.AddComponent<MeshFilter>();
                chunkObject.AddComponent<MeshRenderer>();
                chunkObject.AddComponent<MeshCollider>();
                chunkTransforms[chunkNum] = chunkObject.transform;
            }
        }
    }

    void DeleteChildObjects()
    {
        foreach (Transform c in this.transform)
        {
            DestroyImmediate(c.gameObject);
            if (this.transform.childCount > 0)
            {
                DeleteChildObjects();
            }
        }
    }
}

//[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
