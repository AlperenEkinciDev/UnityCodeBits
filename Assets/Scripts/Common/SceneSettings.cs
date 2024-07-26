using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] Vector3 gravity;
    void Start()
    {
        SetSettings();
    }

    void SetSettings()
    {
        Physics.gravity = this.gravity;
    }
}
