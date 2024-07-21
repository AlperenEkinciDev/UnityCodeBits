using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFallOffMap(int sizeX, int sizeY, float multiplier)
    {
        float[,] map = new float[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                float x = i/ (float)sizeX * 2 - 1;
                float y = j / (float)sizeY * 2 - 1;

                float value = Mathf.Clamp(1f - Mathf.Sqrt(x * x + y * y) * multiplier, 0.0f, 1.0f);
                map[i, j] = value;
            }
        }
        return map;
    }

    static float Evaluate(float value)
    {
        float a = 1f;
        float b = 8.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow((b - b * value), a));
    }
}
