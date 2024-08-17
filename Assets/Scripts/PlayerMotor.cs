using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    public Transform cameraT;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ProcessMove(Vector2 input)
    {

        Vector3 forward = cameraT.forward;
        Vector3 right = cameraT.right;

        forward.y = 0f;
        right.y = 0f;

        Vector3 moveDirection = forward.normalized * input.y + right.normalized * input.x;
        moveDirection.y = -10f;
        controller.Move(moveDirection * speed * Time.deltaTime);

    }

    public void Dash()
    {
        controller.Move(transform.TransformDirection(Vector3.forward) * speed);
    }
}
