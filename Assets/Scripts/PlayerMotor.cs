using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    public Transform cameraT;
    public float speed = 5f;
    private bool isDodge = false;
    private Vector3 dodgeVelocity;
    private float Dframe;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Dframe = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().Dframe;
    }

    void Update()
    {
        if (isDodge)
        {
            controller.Move(dodgeVelocity * Time.deltaTime);
        }
    }

    public void ProcessMove(Vector2 input)
    {
        if (!isDodge)
        {
            Vector3 forward = cameraT.forward;
            Vector3 right = cameraT.right;

            forward.y = 0f;
            right.y = 0f;

            Vector3 moveDirection = forward.normalized * input.y + right.normalized * input.x;
            moveDirection.y = -10f;  // Keep gravity
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    public void Dodge()
    {
        if (!isDodge)
        {
            isDodge = true;

            // Temporarily disable the Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;

            Vector3 dashDirection = transform.TransformDirection(Vector3.forward);
            dodgeVelocity = dashDirection * speed * 7f;

            StartCoroutine(DecayDodgeVelocity(rb));
        }
    }

    private IEnumerator DecayDodgeVelocity(Rigidbody rb)
    {
        float elapsedTime = 0f;
        Vector3 initialVelocity = dodgeVelocity;

        while (elapsedTime < Dframe)
        {
            dodgeVelocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsedTime / Dframe);
            controller.Move(dodgeVelocity * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dodgeVelocity = Vector3.zero;
        isDodge = false;

        // Re-enable the Rigidbody after the dodge
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
}
