using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastReact : MonoBehaviour
{
    [SerializeField] List<Transform> messageTarget;

    //Targets Junction
    public List<Transform> GetMessageTarget()
    {
        return messageTarget;
    }
}
