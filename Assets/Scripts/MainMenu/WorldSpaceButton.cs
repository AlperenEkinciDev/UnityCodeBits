using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour
{
    [SerializeField] Vector3 popUpMovementAmount;
    [SerializeField] float popUpDuration = 0.5f;

    private bool isMouseHovering = false;
    private bool isMouseHoveringLastFrame = false;
    private float stateChangeTime;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float popUpPercentage;
    private int popUpDeactivateFrameCount = 0;

    private void Start()
    {
        startPos = this.transform.position;
        targetPos = startPos + popUpMovementAmount;
        stateChangeTime = -popUpDuration;
    }

    private void Update()
    {
        /*Movement*/
        StateCheck();
        MoveTowardsTarget();
        Timer();
        /*Movement*/



    }


    /********************Movement Methods********************/
    void RaycastActionHit()
    {
        isMouseHovering = true;
        popUpDeactivateFrameCount = 0;
    }

    //Detect state change
    void StateCheck()
    {
        if (isMouseHovering != isMouseHoveringLastFrame)
        {
            isMouseHoveringLastFrame = isMouseHovering;
            stateChangeTime = Time.time;

            if (!isMouseHovering){stateChangeTime -= ( popUpDuration * (1f - popUpPercentage));}else{stateChangeTime -= (popUpDuration * popUpPercentage);}
        }
    }

    //Detects 2 consecutive Raycast Misses
    void Timer()
    {
        if (popUpDeactivateFrameCount > 2)
        {
            isMouseHovering = false;
        }
        popUpDeactivateFrameCount = Mathf.Clamp(popUpDeactivateFrameCount + 1, 0, 3);
    }

    void MoveTowardsTarget()
    {
        popUpPercentage = EaseMethods.EaseOutBounce(Mathf.Clamp((Time.time - stateChangeTime) / popUpDuration, 0.0f, 1.0f)); if (!isMouseHovering) { popUpPercentage = 1 - popUpPercentage; }
        this.transform.position = startPos * (1 - popUpPercentage) + (targetPos * popUpPercentage);
    }
    /********************Movement Methods********************/
}
