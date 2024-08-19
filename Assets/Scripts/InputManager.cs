using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerInput;
    private PlayerControls.GroundActions ground;

    private PlayerMotor playerMotor;
    private PlayerAttack playerAttack;
    private bool canMove = true;
    private GameObject playerHUD;
    // Start is called before the first frame update
    void Awake()
    {
        playerHUD = GameObject.FindGameObjectWithTag("PlayerHUD");
        playerInput = new PlayerControls();
        ground = playerInput.Ground;
        playerMotor = GetComponent<PlayerMotor>();
        playerAttack = GetComponent<PlayerAttack>();
        ground.Attack.performed += ctx => playerAttack.Attack();
        ground.Dodge.performed += ctx =>
        {
            playerMotor.Dodge();
            playerHUD.GetComponent<PlayerHealth>().Dodge();
        };
        ground.Parry.performed += ctx => playerAttack.ParryCheck();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            playerMotor.ProcessMove(ground.Movement.ReadValue<Vector2>());
        }
        else
        {
            playerMotor.ProcessMove(Vector2.zero);
        }

    }

    private void LateUpdate()
    {
    }

    private void OnEnable()
    {
        ground.Enable();
    }
    private void OnDisable()
    {
        ground.Disable();
    }

    public void ToggleMove(bool value)
    {
        canMove = value;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCam>().ToggleTurn(value);
        if (!canMove){    
            playerMotor.ProcessMove(Vector2.zero);
        }
    }

}

