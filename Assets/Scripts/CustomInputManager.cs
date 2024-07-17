using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour
{
    [System.Serializable]
    public class CustomInput
    {
        public string name;

        public KeyCode keyCode_Positive;
        public KeyCode keyCode_Negative;

        public KeyCode alternativeKeyCode_Positive;
        public KeyCode alternativeKeyCode_Negative;

        public bool isToggle;
        public float valueChangePerSecond;
        public float value;
    }
    
    public List<CustomInput> customInputList = new List<CustomInput>();
    public Dictionary<string, CustomInput> customInputDictionary = new Dictionary<string, CustomInput>();

    void Start()
    {
        FillInDictionary();
    }

    void Update()
    {
        UpdateInputValues();
    }

    public float GetCustomInputValue(string name)
    {
        if (customInputDictionary.ContainsKey(name))
        {
            return customInputDictionary[name].value;
        }
        else
        {
            Debug.Log("Custom input with the name: " + name + " does not exist.");
            return 0f;
        }
    }

    void FillInDictionary()
    {
        for (int i = 0; i < customInputList.Count; i++)
        {
            customInputDictionary.Add(customInputList[i].name, customInputList[i]);
        }
    }

    void UpdateInputValues()
    {
        for (int i = 0; i < customInputList.Count; i++)
        {
            if (Input.GetKey(customInputList[i].keyCode_Positive) || Input.GetKey(customInputList[i].alternativeKeyCode_Positive))
            {
                if (customInputList[i].isToggle)
                {
                    customInputList[i].value = 1f;
                }
                else
                {
                    customInputList[i].value += customInputList[i].valueChangePerSecond * Time.deltaTime;
                }
            }
            else if (Input.GetKey(customInputList[i].keyCode_Negative) || Input.GetKey(customInputList[i].alternativeKeyCode_Negative))
            {
                if (customInputList[i].isToggle)
                {
                    customInputList[i].value = -1f;
                }
                else
                {
                    customInputList[i].value -= customInputList[i].valueChangePerSecond * Time.deltaTime;
                }
            }
            else
            {
                if (customInputList[i].isToggle) {
                    customInputList[i].value = Mathf.MoveTowards(customInputList[i].value, 0.0f, 1f);
                }
                else
                {
                    customInputList[i].value = Mathf.MoveTowards(customInputList[i].value, 0.0f, customInputList[i].valueChangePerSecond * Time.deltaTime);
                }
            }

            customInputList[i].value = Mathf.Clamp(customInputList[i].value, -1f, 1f);
        }
    }
}
