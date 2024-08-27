using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverheat : MonoBehaviour
{
    public float maxOverheat = 100f;
    public float overheat;
    public float coolRate;
    public float overheatRate;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Image overheatBar;


    // Start is called before the first frame update
    void Start()
    {
        overheat = 0;
    }

    void FixedUpdate()
    {
        overheat = Mathf.Clamp(overheat, 0, maxOverheat);
        UpdateOverheat();
    }

    public void UpdateOverheat()
    {
        if (overheat != 0 && !playerAttack.holdAttacking) 
        {
            overheat -= Time.deltaTime * coolRate;
        }
        else if (playerAttack.holdAttacking && overheat >= maxOverheat){
            playerAttack.HoldAttack(false);
        }
        float overheatFill = overheatBar.fillAmount;
        float overheatFrac = overheat / maxOverheat;
        if (overheatFill != overheatFrac)
        {
            overheatBar.fillAmount = overheatFrac;
        }
    }

    public void GainOverheat()
    {
        overheat += overheatRate;
    }

}
