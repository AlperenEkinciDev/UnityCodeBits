using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButtonOnClick : MonoBehaviour
{
    [HideInInspector]
    public enum VariableType {Int, Float, String};

    [System.Serializable]
    public struct MessageTarget
    {
        public Transform messageTarget;
        public string message;
        public VariableType variableType;
        [Space]
        public bool sendVariable;
        public int variableInt;
        public float variableFloat;
        public string variableString;
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
        if (customInputManager)
        {
            if (customInputManager.GetCustomInputValue("Interact") > 0.5f && worldSpaceButton.isMouseHovering)
            {
                Interact();
            }
        }
        else
        {
            customInputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<CustomInputManager>();
        }
    }

    private void Interact()
    {
        for (int i = 0; i < messageTargetsList.Count; i++)
        {
            MessageTarget tempTarget = messageTargetsList[i];
            if (tempTarget.sendVariable)
            {
                if(tempTarget.variableType == VariableType.Int) tempTarget.messageTarget.BroadcastMessage(tempTarget.message, tempTarget.variableInt);
                else if (tempTarget.variableType == VariableType.Float) tempTarget.messageTarget.BroadcastMessage(tempTarget.message, tempTarget.variableFloat);
                else if (tempTarget.variableType == VariableType.String) tempTarget.messageTarget.BroadcastMessage(tempTarget.message, tempTarget.variableString);
            }
            else
            {
                tempTarget.messageTarget.BroadcastMessage(tempTarget.message);
            }
        }
    }
}
