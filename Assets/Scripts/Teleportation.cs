using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public GameObject ball;
    public CharacterController controller;
    public RaycastHit hit;
    public Transform cam;
    public float range = 200.0f;


    void Start()
    {
        ball.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit, range))
            {
                ball.transform.position = hit.point;
                ball.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit, range))
            {
                controller.enabled = false;
                controller.transform.position = ball.transform.position + new Vector3(0, 2.0f, 0);
                controller.enabled = true;
                ball.SetActive(false);
            }
        }
    }
}
