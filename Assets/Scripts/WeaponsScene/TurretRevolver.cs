using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRevolver : MonoBehaviour
{
    [System.Serializable]
    public struct RevolverSlot
    {
        public string name;
        public Transform turretControllerTransform;
    }

    [Header("Turret Revolver Communication")]
    [SerializeField] Transform revolverTransform;
    [Space]
    [Header("Turret Revolver Settings")]
    [SerializeField] List<RevolverSlot> revolverSlots;
    [SerializeField] Vector3 revolverFreeLocalAxis;
    [SerializeField] float revolveDuration;

    int currentState = 0;
    Quaternion currentTargetRotation;
    Quaternion stateChangeLocalRotation;
    float stateChangeTime;
    float revolvePercentage;
    int nonChangeFrame = 0;

    private void Start()
    {
        currentTargetRotation = Quaternion.identity;
        stateChangeLocalRotation = currentTargetRotation;
        stateChangeTime = Time.time;
    }

    private void Update()
    {
        RotateTowardsTarget();

        nonChangeFrame++;
    }

    void RotateNext()
    {
        if (nonChangeFrame > 2)
        {
            revolverSlots[currentState].turretControllerTransform.gameObject.SetActive(false);

            currentState++;
            currentState = currentState % revolverSlots.Count;
            stateChangeTime = Time.time;

            currentTargetRotation = Quaternion.AngleAxis(currentState * (360 / revolverSlots.Count), revolverFreeLocalAxis);
            stateChangeLocalRotation = revolverTransform.localRotation;
        }

        nonChangeFrame = 0;
    }

    void RotateTowardsTarget()
    {
        revolvePercentage = EaseMethods.EaseOutBounce(Mathf.Clamp((Time.time - stateChangeTime) / revolveDuration, 0.0f, 1.0f));
        revolverTransform.localRotation = Quaternion.Slerp(stateChangeLocalRotation, currentTargetRotation, revolvePercentage);

        if (1f - revolvePercentage < 0.025f)
        {
            revolverSlots[currentState].turretControllerTransform.gameObject.SetActive(true);
        }
    }
}
