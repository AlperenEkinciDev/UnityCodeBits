using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycaster : MonoBehaviour
{
    private RaycastHit mouseRayHit = new RaycastHit();
    private Transform lastHitTransform;
    private RaycastReact raycastReact;
    private RaycastReact lastRaycastReact;

    private void Update()
    {
        if (CheckRaycast())
        {
            ActOnHit();
        }
    }

    //Broadcast Message To Target
    private void ActOnHit()
    {
        if (mouseRayHit.transform.TryGetComponent<RaycastReact>(out raycastReact))
        {
            List<Transform> targetList = raycastReact.GetMessageTarget();

            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].BroadcastMessage("RaycastActionHit");
            }
        }
    }

    //Send A Raycast Through Camera Towards Mouse Projection
    private bool CheckRaycast()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out mouseRayHit))
        {
            return true;
        }

        return false;
    }
}
