using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButtonMove : MonoBehaviour
{
    [Header("World Space Button Move Settings")]
    [SerializeField] WorldSpaceButton worldSpaceButton;
    [SerializeField] Vector3 popUpMovementAmount;
    [SerializeField] float popUpDuration = 0.5f;

    private bool isMouseHovering = false;
    private bool isMouseHoveringLastFrame = false;
    private float stateChangeTime;
    private Vector3 startLocalPos;
    private Vector3 targetPos;
    private float popUpPercentage;

    private void Start()
    {
        startLocalPos = this.transform.localPosition;
        targetPos = startLocalPos + popUpMovementAmount;
        stateChangeTime = -popUpDuration;
    }


    private void Update()
    {
        UpdateValues();
        MoveTowardsTarget();
    }


    //Communicate with root script
    private void UpdateValues()
    {
        isMouseHovering = worldSpaceButton.isMouseHovering;

        if (isMouseHoveringLastFrame != isMouseHovering) {
            isMouseHoveringLastFrame = isMouseHovering;
            stateChangeTime = Time.time;
            if (!isMouseHovering) { stateChangeTime -= (popUpDuration * (1f - popUpPercentage)); } else { stateChangeTime -= (popUpDuration * popUpPercentage); }
        }
    }


    //Eased movement
    void MoveTowardsTarget()
    {
        popUpPercentage = EaseMethods.EaseOutBounce(Mathf.Clamp((Time.time - stateChangeTime) / popUpDuration, 0.0f, 1.0f)); if (!isMouseHovering) { popUpPercentage = 1 - popUpPercentage; }
        this.transform.localPosition = startLocalPos * (1 - popUpPercentage) + (targetPos * popUpPercentage);
    }
}
