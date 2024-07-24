using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockGlobalRot : MonoBehaviour
{
    [Header("Unlocked Axis")]
    [SerializeField] Vector3 axis;
    void FixedUpdate()
    {
        this.transform.eulerAngles = Vector3.Scale(this.transform.eulerAngles, axis);
    }
}
