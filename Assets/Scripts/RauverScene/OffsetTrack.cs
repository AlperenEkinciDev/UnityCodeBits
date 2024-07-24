using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetTrack : MonoBehaviour
{
    [SerializeField] Transform TrackTarget;
    [SerializeField] int state;
    Vector3 offset;
    void Start()
    {
        if (state == 1) { 
            Vector3 targetDir = (TrackTarget.position - this.transform.position).normalized;
            offset = (this.transform.forward - targetDir);
        }
        else if (state == -1)
        {
            Vector3 targetDir = (TrackTarget.position - this.transform.position).normalized;
            offset = (-this.transform.forward - targetDir);
        }
    }


    void Update()
    {
        if (state == 1)
        {
            this.transform.forward = (TrackTarget.position - this.transform.position).normalized + offset;
        }

        else if (state == -1)
        {
            this.transform.forward = -((TrackTarget.position - this.transform.position).normalized + offset);
        }
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, 0.0f, 0.0f);
    }
}
