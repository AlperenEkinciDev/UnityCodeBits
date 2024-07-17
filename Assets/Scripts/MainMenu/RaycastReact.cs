using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastReact : MonoBehaviour
{
    [SerializeField] List<Transform> messageTarget;

    public List<Transform> GetMessageTarget()
    {
        return messageTarget;
    }
}
