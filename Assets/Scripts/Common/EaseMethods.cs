using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EaseMethods
{
    //CONST
    const float PI = Mathf.PI;
    const float C4 = (2 * PI) / 3;



    public static float EaseOutElastic(float x) 
    {
        if (x == 0 || x == 1) { return x;}
        else {return Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * C4) + 1;}
    }


    public static float EaseOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }



    public static float EaseOutQuad(float x)
    {
        if (x < 0.5f)
        {
            return 2 * x * x;
        }
        else
        {
            return 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        }
    }
}
