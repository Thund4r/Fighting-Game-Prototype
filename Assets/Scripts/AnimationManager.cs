using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerOverheat player1HUD;
    // Start is called before the first frame update
    private void Start()
    {
    }
    public void BeginFaceEnemy()
    {
        playerAttack.BeginFaceEnemy();
    }

    public void EndFaceEnemy()
    {
        playerAttack.EndFaceEnemy();
    }

    public void AttackCooldown()
    {
        playerAttack.AttackCooldown();
    }

    public void GainOverheat()
    {
        player1HUD.GainOverheat();
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

    public void MeleeDodge()
    {
        playerAttack.MeleeDodge();
    }

    public void MeleeDodge2()
    {
        playerAttack.MeleeDodge2();
    }
}
