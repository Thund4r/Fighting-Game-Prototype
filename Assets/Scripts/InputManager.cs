using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerInput;
    private PlayerControls.GroundActions ground;

    private PlayerMotor playerMotor;
    //private PlayerLook playerLook;
    //private PlayerShoot playerShoot;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerControls();
        ground = playerInput.Ground;
        playerMotor = GetComponent<PlayerMotor>();
        //playerLook = GetComponent<PlayerLook>();
        //playerShoot = GetComponent<PlayerShoot>();
        //ground.Shoot.performed += ctx => playerShoot.Shoot();
        //ground.ShootEnergy.performed += ctx => playerShoot.ShootEnergy();
        //ground.Dash.performed += ctx => playerMotor.Dash();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerMotor.ProcessMove(ground.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        //playerLook.ProcessLook(ground.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        ground.Enable();
    }
    private void OnDisable()
    {
        ground.Disable();
    }

}

