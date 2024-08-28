using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image RedHealth;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private Transform HUDTransform;

    void LateUpdate()
    {
        HUDTransform.LookAt(Camera.main.transform);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        
        float health = enemyMovement.health;
        float maxHP = enemyMovement.maxHP;
        float healthFill = RedHealth.fillAmount;
        float healthFrac = health / maxHP;
        if (healthFill > healthFrac)
        {
            RedHealth.fillAmount = healthFrac;
        }
    }
}
