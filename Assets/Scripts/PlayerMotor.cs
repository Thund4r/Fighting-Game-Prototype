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
    public bool perfectDodge = false;
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

            if (moveDirection != Vector3.zero)
            {
                // Rotate the player to face the direction of movement
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            if (!controller.isGrounded)
            {
                moveDirection.y = -9.8f;
            }

            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    public void Dodge()
    {
        if (!isDodge)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider hit in colliders)
            {

                if (hit.name == "EnemyObj")
                {
                    if (hit.GetComponent<EnemyMovement>().isAttacking)
                    {
                        Time.timeScale = 0.3f;
                        perfectDodge = true;
                    }
                }
            }
            isDodge = true;
            Rigidbody rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;

            Vector3 dashDirection = transform.TransformDirection(Vector3.forward);
            dodgeVelocity = dashDirection * speed * 5f;

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
        Time.timeScale = 1f;
        dodgeVelocity = Vector3.zero;
        isDodge = false;
        perfectDodge = false;

        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
}
