using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj;

    public float rotationSpeed;
    public bool canTurn = true;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // All the code below is currently redundant
   /* void FixedUpdate()
    {
        if (canTurn)
        {
            // Find "Forward" relative to the camera
            Vector3 viewDir = playerObj.position - new Vector3(transform.position.x, playerObj.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                // Slerp linearly interpolates between the direction the obj is facing, and the input direction, at the rate of rotation speed, turning the obj to face input dir
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }


    }

    public void ToggleTurn(bool value)
    {
        canTurn = value;
    }

    public void Zoom(float zoom)
    {
       GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = zoom;
    }*/
}
