using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [HideInInspector]
    public float currentDistance;
    [HideInInspector]
    public CarController carController;
    [HideInInspector]
    public float currentSuspensionVelocity;

    [Header("Wheel Communication")]
    public Transform rayPosition;
    [Header("Wheel Settings")]
    public float wheelSize;
    public float wheelTrackSpeed = 2f;

    private float lastFrameDistance;
    private RaycastHit hit;
    private int layermask = 64; //...100000 -> 6th layer
    private Vector3 startLocalPos;
    private Vector3 currentTargetPos;

    private void Start()
    {
        startLocalPos = this.transform.localPosition + Vector3.up * (carController.suspensionMaxDistance/2f);
    }

    void FixedUpdate()
    {
        UpdateValues();
        PlaceWheel();
    }

    void UpdateValues()
    {
        if (Physics.Raycast(rayPosition.position, Vector3.down, out hit, Mathf.Infinity, layermask, QueryTriggerInteraction.Ignore))
        {
            currentDistance = Vector3.Distance(rayPosition.position, hit.point);
        }
        else
        {
            currentDistance = Mathf.Infinity;
        }

        currentSuspensionVelocity = currentDistance - lastFrameDistance;
        lastFrameDistance = currentDistance;
    }

    void PlaceWheel()
    {
        currentTargetPos = startLocalPos + Vector3.down * (Mathf.Min(carController.suspensionMaxDistance, currentDistance) - wheelSize);

        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, currentTargetPos, wheelTrackSpeed * Time.deltaTime);
    }
}
