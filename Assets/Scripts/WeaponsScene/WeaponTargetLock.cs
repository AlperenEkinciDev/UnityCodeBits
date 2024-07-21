using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTargetLock : Weapon
{
    /*Component Properties*/
    [System.Serializable]
    public struct Component
    {
        public Transform weaponComponent;
        public Vector3 freeAxis;
    }
    /*Component Properties*/

    //
    [Header("Weapon Components")]
    [SerializeField] List<Component> componentList = new List<Component>();
    [Space]
    [Header("Track Properties")]
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField][Tooltip("How much closer the new target cannidate neets to be for target change.")] float differenceThreshold = 1f;
    [SerializeField] public Vector3 trackOffset;
    [SerializeField] public float trackLimit;
    //

    private Vector3 startLocalDir;
    private GameObject[] targetsParentArray;
    Transform targetsParent;

    private void Start()
    {
        targetsParentArray = GameObject.FindGameObjectsWithTag("TargetsParent");

        startLocalDir = this.transform.InverseTransformDirection(this.transform.forward);
    }

    void Update()
    {
        FindClosestTrackTarget();
        TrackTarget();
    }

    void TrackTarget()
    {
        for (int i = 0; i < componentList.Count; i++)
        {
            ComponentTrackTargetRotation(componentList[i].weaponComponent, componentList[i].freeAxis, targetTransform);
        }
    }

    void ComponentTrackTargetRotation(Transform componentTransform, Vector3 componentFreeAxis, Transform targetTransform)
    {
        //Full interpolated track
        Vector3 dir = this.transform.TransformDirection(startLocalDir);
        if (targetTransform)
        {
            dir = ((targetTransform.position + trackOffset) - componentTransform.position).normalized;
        }
        Quaternion rot = Quaternion.LookRotation(dir);
        rot = Quaternion.Lerp(componentTransform.rotation, rot, rotationSpeed * Time.deltaTime);
        componentTransform.rotation = rot;
       

        //Restrict track
        componentTransform.localRotation = Quaternion.Euler(Vector3.Scale(componentTransform.localRotation.eulerAngles, componentFreeAxis));
    }

    void FindClosestTrackTarget()
    {
        float currentDistance = Mathf.Infinity;

        for (int k = 0; k < targetsParentArray.Length; k++)
        {
            targetsParent = targetsParentArray[k].transform;

            for (int i = 0; i < targetsParent.childCount; i++)
            {
                Transform tempChild = targetsParent.GetChild(i);

                float testDistance = Vector3.Distance(this.transform.position, tempChild.position);

                if (testDistance  + differenceThreshold < currentDistance)
                {
                    targetTransform = tempChild;
                    currentDistance = testDistance;
                }
            }
        }
    }
}
