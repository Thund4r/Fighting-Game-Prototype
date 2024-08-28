using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerParry : MonoBehaviour
{
    private int maxParry = 8;
    public float parryCount;
    [SerializeField] private Image ParryBar;


    // Start is called before the first frame update
    void Start()
    {
        parryCount = maxParry;
    }

    public void LoseParry()
    {
        parryCount--;
        float parryFill = ParryBar.fillAmount;
        float parryFrac = parryCount/maxParry;
        if (parryFill != parryFrac)
        {
            ParryBar.fillAmount = parryFrac;
        }
    }

    public void GainParry(int value)
    {
        parryCount = Mathf.Clamp(parryCount + value, 0, maxParry);
        float parryFill = ParryBar.fillAmount;
        float parryFrac = parryCount/maxParry;
        if (parryFill != parryFrac)
        {
            ParryBar.fillAmount = parryFrac;
        }
    }

}
