using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    private PlayerOverheat playerHUD;
    // Start is called before the first frame update
    private void Start()
    {
        playerHUD = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerOverheat>();
    }
    public void BeginFaceEnemy()
    {
        playerAttack.BeginFaceEnemy();
    }

    public void EndFaceEnemy()
    {
        playerAttack.EndFaceEnemy();
    }

    public void GainOverheat()
    {
        playerHUD.GainOverheat();
    }

    public void BeginComboFinisher()
    {
        playerAttack.SetComboFinisher(true);
    }

    public void EndComboFinisher() 
    {
        playerAttack.SetComboFinisher(false);
    }

    public void RangedBasic1()
    {
        playerAttack.RangedBasic1();
    }

    public void RangedBasic2()
    {
        playerAttack.RangedBasic2();
    }

    public void RangedBasicEnd()
    {
        playerAttack.RangedBasicEnd();
    }

    public void RangeDodge()
    {
        playerAttack.RangeDodge();
    }
}
