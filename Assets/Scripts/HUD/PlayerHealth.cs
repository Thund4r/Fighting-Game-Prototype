using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float maxHP = 100f;
    private float health;
    private float Itimer;
    private float Dtimer;
    public Image RedHealth;
    public float Iframe = 1f;
    public float Dframe = 0.4f;
    public float PDframe = 0.8f;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

        if (Itimer > 0)
        {
            Itimer -= Time.deltaTime;
        }
        if (Dtimer > 0)
        {
            Dtimer -= Time.deltaTime;
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
        if (Itimer <= 0 && Dtimer <= 0)
        {
            health -= damage;
            Itimer = Iframe;
        }

    }

    public void Dodge()
    {
        if (GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerMotor>().perfectDodge)
        {
            Dtimer = PDframe;
        }
        else { Dtimer = Dframe; }

        }
}
