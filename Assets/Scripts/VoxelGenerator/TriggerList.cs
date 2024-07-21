using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerList : MonoBehaviour
{
    List<Transform> triggerTransformList = new List<Transform>();


    private void Update()
    {
        Debug.Log(triggerTransformList.Count);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!triggerTransformList.Contains(collision.transform) && collision.collider.isTrigger)
        {
            triggerTransformList.Add(collision.transform);
        }
    }

    public List<Transform> GetList()
    {
        return triggerTransformList;
    }
}
