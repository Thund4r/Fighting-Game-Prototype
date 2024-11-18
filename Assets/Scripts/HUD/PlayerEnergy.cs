using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private float maxMP = 100f;
    public float energy;
    [SerializeField] private Image EnergyBar;


    // Start is called before the first frame update
    void Start()
    {
        energy = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        energy = Mathf.Clamp(energy, 0, maxMP);
        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        float energyFill = EnergyBar.fillAmount;
        float energyFrac = energy / maxMP;
        if (energyFill != energyFrac)
        {
            EnergyBar.fillAmount = energyFrac;
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
}
