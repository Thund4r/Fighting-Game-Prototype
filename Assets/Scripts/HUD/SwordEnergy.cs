using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class SwordEnergy : MonoBehaviour
{
    private int maxSwordLevel = 3;
    
    private Color level1 = new Color32(0, 160, 255, 255);
    private Color level2 = new Color32(248, 188, 57, 255);
    private Color level3 = new Color32(255, 56, 47, 255);
    private bool blink = false;
    [SerializeField] private WeaponCollision weaponCollision;

    public int swordLevel;
    public float energy;
    public float energyThresh = 50f;

    [SerializeField] private Image SwordBlade;
    [SerializeField] private Image SwordBase;


    // Start is called before the first frame update
    void Start()
    {
        energy = 0;
        swordLevel = 1;
        SwordBlade.color = level1;
        SwordBase.color = level1; 
    }

    void FixedUpdate()
    {
        energy = Mathf.Clamp(energy, 0, energyThresh);
        UpdateEnergy();

    }

    private void UpdateEnergy()
    {
        float energyFill = SwordBlade.fillAmount;
        float energyFrac = energy / energyThresh;
        if (energyFill != energyFrac)
        {
            SwordBlade.fillAmount = energyFrac;
        }

        if (energyFrac == 1)
        {
            blink = true;
        }
        else
        {
            blink = false;
        }
    }

    public void GainEnergy(float energyGain)
    {
        energy += energyGain;
    }

    public void LoseEnergy(float energyLoss)
    {
        energy -= energyLoss;
    }

    public void GainLevel()
    {
        swordLevel += 1;
        swordLevel = Mathf.Clamp(swordLevel, 1, maxSwordLevel);
        Debug.Log(SwordBase.color);
        switch (swordLevel)
        {
            case 1:
                SwordBlade.color = level1;
                SwordBase.color = level1;
                break;

            case 2:
                SwordBlade.color = level2;
                SwordBase.color = level2;
                break;
            
            case 3:
                SwordBlade.color = level3;
                SwordBase.color = level3;
                break;
        }
        Debug.Log(SwordBase.color);
    }

    public void LoseLevel(int amount)
    {
        swordLevel -= amount;
        swordLevel = Mathf.Clamp(swordLevel, 1, maxSwordLevel);
        switch (swordLevel)
        {
            case 1:
                SwordBlade.color = level1;
                SwordBase.color = level1;
                break;

            case 2:
                SwordBlade.color = level2;
                SwordBase.color = level2;
                break;

            case 3:
                SwordBlade.color = level3;
                SwordBase.color = level3;
                break;
        }
    }

    private IEnumerator BlinkingBlade()
    {
        Color originalColor = SwordBase.color; 
        Color blinkColor = Color.white;       
        float blinkDuration = 0.5f;          

        while (blink)
        {
            float elapsedTime = 0f;
            while (elapsedTime < blinkDuration)
            {
                SwordBase.color = Color.Lerp(originalColor, blinkColor, elapsedTime / blinkDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < blinkDuration)
            {
                SwordBase.color = Color.Lerp(blinkColor, originalColor, elapsedTime / blinkDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        SwordBase.color = originalColor;
    }


    public void SetSwordFinisher(bool value)
    {
        weaponCollision.swordFinisher = value;
    }

}
