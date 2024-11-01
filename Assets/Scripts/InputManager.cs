using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerInput;
    private PlayerControls.GroundActions ground;
    private PlayerControls.ChainAttackActions chain;

    private PlayerMotor playerMotor;
    private PlayerAttack playerAttack;
    private bool canMove = true;
    private GameObject playerHUD;
    [SerializeField] private CharManager charManager;
    // Start is called before the first frame update
    void Awake()
    {
        playerHUD = GameObject.FindGameObjectWithTag("PlayerHUD");
        playerInput = new PlayerControls();
        ground = playerInput.Ground;
        chain = playerInput.ChainAttack;
        playerMotor = GetComponent<PlayerMotor>();
        playerAttack = GetComponent<PlayerAttack>();
        ground.Attack.performed += ctx => playerAttack.Attack();
        ground.HoldAttack.performed += ctx => playerAttack.HoldAttack(true);
        ground.HoldAttack.canceled += ctx => playerAttack.HoldAttack(false);
        ground.Dodge.performed += ctx =>
        {
            playerMotor.Dodge(ground.Movement.ReadValue<Vector2>(), 5f, GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().Dframe);
            playerHUD.GetComponent<PlayerHealth>().Dodge();
        };
        //ground.Parry.performed += ctx => playerAttack.ParryCheck();   --- OLD PARRY
        ground.Parry.performed += ctx => charManager.SwapNextChar();
        ground.Sprint.performed += ctx => playerMotor.Sprint(true);
        ground.Sprint.canceled += ctx => playerMotor.Sprint(false);
        ground.ExSpecial.performed += ctx => playerAttack.ExSpecialCheck(playerHUD);
        chain.Character1.performed += ctx => StartCoroutine(playerAttack.TriggerChainAttack());

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
        
        if (!canMove){    
            playerMotor.ProcessMove(Vector2.zero);
        }
    }

    public void EnableChainAttack()
    {
        playerInput.ChainAttack.Enable();
        playerInput.Ground.Disable();
    }

    public void DisableChainAttack()
    {
        playerInput.ChainAttack.Disable();
        playerInput.Ground.Enable();
    }

}

