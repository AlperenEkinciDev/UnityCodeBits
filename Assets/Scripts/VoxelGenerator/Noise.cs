using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidh, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidh, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) {
            scale = 0.001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidh / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidh; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidh; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
       return noiseMap;
    }

    public static float[,,] GenerateNoiseMap3D(int cubeMapWidth, int cubeMapHeight, int cubeMapDepth, int detailMultiplier, int seed, float scale, int octaves, float persistance, float lacunarity, bool useFalloff, float multiplier, bool useDegredation, float degredationRate, Vector2 offset, int fillLayerLowLimit, int fillLayerHighLimit, float fillValue, bool useFillLayer)
    {
        cubeMapWidth *= detailMultiplier;
        cubeMapHeight *= detailMultiplier;
        cubeMapDepth *= detailMultiplier;
        scale *= (float)detailMultiplier;

        float[,] noiseMap1 = GenerateNoiseMap(cubeMapWidth , cubeMapHeight, seed, scale, octaves, persistance, lacunarity, offset);
        float[,] noiseMap2 = GenerateNoiseMap(cubeMapDepth, cubeMapHeight, seed + 1, scale, octaves, persistance, lacunarity, offset);
        float[,] noiseMap3 = GenerateNoiseMap(cubeMapWidth, cubeMapDepth, seed + 2, scale, octaves, persistance, lacunarity, offset);

        float[,] falloffMap = FalloffGenerator.GenerateFallOffMap(cubeMapWidth, cubeMapDepth, multiplier);

        float[,,] noiseMap3D = new float[cubeMapDepth, cubeMapHeight, cubeMapWidth];

        float maxNoiseDensity = float.MinValue;
        float minNoiseDensity = float.MaxValue;

        for (int d = 0; d < cubeMapDepth; d++)//d:Depth, h:Height, w:Width
        {
            for (int h = 0; h < cubeMapHeight; h++)
            {
                for (int w = 0; w < cubeMapWidth; w++)
                {
                    float density = noiseMap1[w, h] * noiseMap2[d, h] * noiseMap3[w, d];
                    noiseMap3D[d, h, w] = density;

                    if (density > maxNoiseDensity)
                    {
                        maxNoiseDensity = density;
                    }
                    else if (density < minNoiseDensity)
                    {
                        minNoiseDensity = density;
                    }

                    
                    if (useFalloff) {
                        density = density* falloffMap[w, d];
                        noiseMap3D[d, h, w] = density;
                    }
                    if (useDegredation)
                    {
                        density = density * Mathf.Clamp((h / cubeMapHeight) + (1 - degredationRate), 0.0f, 1.0f);
                        noiseMap3D[d, h, w] = density;
                    }
                    if (useFillLayer && h >= fillLayerLowLimit && h <= fillLayerHighLimit)
                    {
                        density = 1.0f;
                        noiseMap3D[d, h, w] = fillValue;
                    }
                }
            }
        }
        for (int d = 0; d < cubeMapDepth; d++)//d:Depth, h:Height, w:Width
        {
            for (int h = 0; h < cubeMapHeight; h++)
            {
                for (int w = 0; w < cubeMapWidth; w++)
                {
                    noiseMap3D[d, h, w] = Mathf.InverseLerp(minNoiseDensity, maxNoiseDensity, noiseMap3D[d, h, w]);
                }
            }
        }

        return noiseMap3D;
    }
}
