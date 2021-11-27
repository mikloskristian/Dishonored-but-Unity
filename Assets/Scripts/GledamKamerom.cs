using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GledamKamerom : MonoBehaviour
{

    public Transform Korvo;

    public float MouseSensitivity = 100.0f;

    float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        Korvo.Rotate(Vector3.up * mouseX);
    }
}
