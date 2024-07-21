using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour
{
    [HideInInspector]
    public bool isMouseHovering = false;
    private bool isMouseHoveringLastFrame = false;
    private int popUpDeactivateFrameCount = 0;

    void Update()
    {
        StateCheck();
        Timer();
    }

    //Message target
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
}
