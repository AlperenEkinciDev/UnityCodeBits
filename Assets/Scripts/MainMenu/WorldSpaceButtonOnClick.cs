using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButtonOnClick : MonoBehaviour
{
    [System.Serializable]
    public struct MessageTarget
    {
        public Transform messageTarget;
        public string message;
        [Space]
        public bool sendVariable;
        public int variable;
    }


    [Header("World Space Button Communication")]
    [SerializeField] WorldSpaceButton worldSpaceButton;
    [Header("World Space Button Action")]
    [SerializeField] List<MessageTarget> messageTargetsList = new List<MessageTarget>();

    private CustomInputManager customInputManager;
    void Start()
    {
        customInputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<CustomInputManager>();
    }


    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (customInputManager.GetCustomInputValue("Interact") > 0.5f && worldSpaceButton.isMouseHovering)
        {
            Interact();
        }
    }

    private void Interact()
    {
        for (int i = 0; i < messageTargetsList.Count; i++)
        {
            MessageTarget tempTarget = messageTargetsList[i];
            if (tempTarget.sendVariable)
            {
                tempTarget.messageTarget.BroadcastMessage(tempTarget.message, tempTarget.variable);
            }
            else
            {
                tempTarget.messageTarget.BroadcastMessage(tempTarget.message);
            }
        }
    }
}
