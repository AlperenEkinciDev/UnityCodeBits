using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    [SerializeField] Transform startTransform, endTransform, laserLine;
    [SerializeField] float flickerScaleLow, flickerScaleHigh;
    [SerializeField] Vector3 startOffset, endOffset;

    bool isBlocked;
    Vector3 startScaleLaserLine;
    RaycastHit hit;
    void Start()
    {
        startScaleLaserLine = laserLine.localScale;
    }
    void Update()
    {
        PlaceStartEnd();
        UpdateLaserLine();
    }


    Vector3 CheckObstacle()
    {
        
        isBlocked = false;

        if (Physics.Raycast(startTransform.position, (targetTransform.position - startTransform.position).normalized, out hit))
        {
            isBlocked = true;
        }

        return hit.point;
    }

    void PlaceStartEnd()
    {
        startTransform.position = spawnPointTransform.position + startTransform.TransformDirection(startOffset);
        endTransform.position = CheckObstacle() + startTransform.TransformDirection(endOffset);
    }

    void UpdateLaserLine()
    {
        laserLine.position = (startTransform.position + endTransform.position) / 2f;
        laserLine.localScale = new Vector3(startScaleLaserLine.x * Random.Range(flickerScaleLow, flickerScaleHigh), (startTransform.position - endTransform.position).magnitude / 2f, startScaleLaserLine.z * Random.Range(flickerScaleLow, flickerScaleHigh));
        laserLine.up = (startTransform.position - endTransform.position).normalized;
    }
}
