using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float maxHP = 100f;
    private float health;
    private float timer;
    public Image RedHealth;
    public float Iframe = 1f;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        health = Mathf.Clamp(health, 0, maxHP);
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        float healthFill = RedHealth.fillAmount;
        float healthFrac = health / maxHP;
        if (healthFill > healthFrac)
        {
            RedHealth.fillAmount = healthFrac;
        }
    }

    public void TakeDamage(float damage)
    {
        if (timer <= 0)
        {
            health -= damage;
            timer = Iframe;
        }

    }
}
