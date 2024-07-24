using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("CarController Communication")]
    [SerializeField] List<Wheel> wheelList = new List<Wheel>();
    [Header("CarController Settings")]
    public float suspensionMaxDistance;
    [SerializeField] float suspensionStrength;
    [SerializeField] float suspensionMaxDampingSpeed;

    private Rigidbody thisRigidbody;
    private float wheelCount;
    void Awake()
    {
        thisRigidbody = this.transform.GetComponent<Rigidbody>();
        wheelCount = wheelList.Count;

        for (int i = 0; i < wheelCount; i++)
        {
            wheelList[i].carController = this;
        }
    }

    void FixedUpdate()
    {
        AddSuspensionForces();
    }

    void AddSuspensionForces()
    {
        for (int i = 0; i < wheelCount; i++)
        {
            ApplyForce(wheelList[i]);
        }
    }

    void ApplyForce(Wheel w)
    {
        if (w.currentDistance <= suspensionMaxDistance)
        {
            float forcePercentage = Mathf.Clamp(((1f - (w.currentDistance / suspensionMaxDistance))), 0.0f, 1.0f);
            float damping = Mathf.Clamp(w.currentSuspensionVelocity / (suspensionMaxDampingSpeed * Time.fixedDeltaTime), 0.0f, 1.0f);

            forcePercentage *= (1f - damping);

            thisRigidbody.AddForceAtPosition(Vector3.up * suspensionStrength * forcePercentage / wheelCount, w.transform.position);
        }
    }
}
