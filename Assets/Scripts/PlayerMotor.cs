using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private CharManager charManager;
    private CharacterController controller;
    public Transform cameraT;
    private float speed;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    private bool isDodge = false;
    public bool perfectDodge = false;
    private float DIframe = 1.5f;
    private Vector3 dodgeVelocity;

    void Start()
    {
        speed = walkSpeed;
        controller = GetComponent<CharacterController>();
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
                this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            if (!controller.isGrounded)
            {
                moveDirection.y = -9.8f;
            }

            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    public void Sprint(bool value)
    {
        if (value) {speed = sprintSpeed;}
        else {speed = walkSpeed;}
    }

    public void Dodge(Vector2 input, float dodgeSpeed, float dodgeTime)
    {
        if (!isDodge)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider hit in colliders)
            {
                if (hit.name == "EnemyObj")
                {
                    if (hit.GetComponent<EnemyMovement>().isPerfectDodge)
                    {
                        Time.timeScale = 0.3f;
                        perfectDodge = true;
                    }
                }
            }

            isDodge = true;
            Rigidbody rb = charManager.activeChar.transform.Find("PlayerObj").GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;
            Vector3 forward = cameraT.forward;
            Vector3 right = cameraT.right;

            forward.y = 0f; 
            right.y = 0f;   

            Vector3 dashDirection = (forward * input.y + right * input.x).normalized;

            dodgeVelocity = dashDirection * dodgeSpeed * 5f;

            StartCoroutine(DecayDodgeVelocity(rb, dodgeTime));
        }
    }

    public void AnimDodge(Vector2 input, float dodgeSpeed, float dodgeTime)
    {
        if (!isDodge)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider hit in colliders)
            {
                if (hit.name == "EnemyObj")
                {
                    if (hit.GetComponent<EnemyMovement>().isPerfectDodge)
                    {
                        Time.timeScale = 0.3f;
                        perfectDodge = true;
                    }
                }
            }

            isDodge = true;
            Rigidbody rb = charManager.activeChar.transform.Find("PlayerObj").GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;

            Vector3 dashDirection = (this.transform.forward * input.y + this.transform.right * input.x).normalized;

            dodgeVelocity = dashDirection * dodgeSpeed * 5f;

            StartCoroutine(DecayDodgeVelocity(rb, dodgeTime));
        }
    }

    private IEnumerator DecayDodgeVelocity(Rigidbody rb, float dodgeTime)
    {
        float elapsedTime = 0f;
        Vector3 initialVelocity = dodgeVelocity;

        while (elapsedTime < dodgeTime)
        {
            dodgeVelocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsedTime / dodgeTime);
            controller.Move(dodgeVelocity * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        dodgeVelocity = Vector3.zero;
        isDodge = false;
        perfectDodge = false;

        while (elapsedTime < DIframe)
        {
            elapsedTime += Time.deltaTime;
        }
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }
}
