using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public Transform player;
    public float horizontalSpeed =2;
    public float verticalSpeed=2;
     float yaw;
     float pitch;

    private void Update()
    {
        transform.LookAt(player);
        yaw += horizontalSpeed * Input.GetAxis("Mouse X");
        pitch -= verticalSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        player.transform.Rotate(0, yaw, 0); 
    }

}
