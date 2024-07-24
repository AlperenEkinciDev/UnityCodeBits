using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Camera And Axis Transforms")]
    [SerializeField] Transform vertical;
    [SerializeField] Transform horizontal;
    [SerializeField] Camera camera;
    [Header("Orbit Settings")]
    [SerializeField] float orbitSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float handleSpeed;

    private CustomInputManager customInputManager;

    void Start()
    {
        customInputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<CustomInputManager>();
    }

    void Update()
    {
        if (customInputManager.GetCustomInputValue("Rotate") > 0.9f)
        {
            if(customInputManager.GetCustomInputValue("Handle") > 0.9f)
            {
                Handle();
            }
            else
            {
                Orbit();
            }
        }

        Zoom();
    }

    void Orbit()
    {
            float horizontalInput = -Input.GetAxis("Mouse Y") * orbitSpeed * Time.deltaTime;
            float verticalInput = Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;

            if (vertical)
            {
                float y = vertical.localEulerAngles.y + verticalInput;
                vertical.localEulerAngles = new Vector3(0.0f, y, 0.0f);
            }
            if (horizontal)
            {
                float x = horizontal.localEulerAngles.x + horizontalInput;
                horizontal.localEulerAngles = new Vector3(x, 0.0f, 0.0f);
            }
    }

    void Handle()
    {
        float horizontalInput = -Input.GetAxis("Mouse X") * handleSpeed * Time.deltaTime;
        float verticalInput = -Input.GetAxis("Mouse Y") * handleSpeed * Time.deltaTime;

        vertical.transform.position += camera.transform.right * horizontalInput + camera.transform.up * verticalInput;
    }

    void Zoom()
    {
        float zoomInput = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;

        if (camera.orthographic)
        {
            camera.orthographicSize -= zoomInput;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 0.1f, Mathf.Infinity);
        }
        else
        {
            camera.transform.position += camera.transform.forward * zoomInput;
        }
    }
}
