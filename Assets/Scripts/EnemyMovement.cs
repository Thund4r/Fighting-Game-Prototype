using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed;
    public GameObject agent;
    public EnemyAttack attack;
    public float aggro;
    public float maxHP;
    public float health;
    public float maxDaze;
    public float daze;
    public float stunDuration;
    public float dmgMult;
    public bool isParryable = false;
    public bool canMove = true;
    public bool isAttacking = false;
    public bool isPerfectDodge = false;
    public bool isStunned = false;
    public bool isChained = true;


    private float timer;
    private GameObject player;
    private float distance;  
    private bool alive = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHP;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;
        if (alive && canMove && !isStunned)
        {
            agent.transform.LookAt(player.transform);
            distance = (player.transform.position - transform.position).sqrMagnitude;
            agent.GetComponent<Rigidbody>().AddForce(agent.transform.forward * (movementSpeed * (Mathf.Sqrt(distance))/5 ));
            
            if (distance < aggro)
            {
                attackRoutine();
                timer = 0;
            }
        }
        else if (isStunned)
        {
            StunUpdate();
        }
    }

    public void TakeDaze(int value)
    {
        if (!isStunned)
        {
            daze = Mathf.Clamp(daze + value, 0, maxDaze);
        }
        
        if (daze == maxDaze)
        {
            isStunned = true;
            isChained = false;
            dmgMult = 2f;
        }
    }

    public void StunUpdate()
    {
        daze = Mathf.Clamp(daze - ((Time.deltaTime / stunDuration) * maxDaze), 0, maxDaze);
        if (daze == 0)
        {
            isStunned = false;
            isChained = true;
            dmgMult = 1f;
        }
        
    }

    public void TakeDamage(int damage)
    {

        health -= damage * dmgMult;
        if (canMove || isAttacking)
        {
            this.attack.timer = Mathf.Clamp(this.attack.timer - (this.attack.timer * (health/(maxHP*8)) + 0.07f), 0, 1000);
            Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
            agent.GetComponent<Rigidbody>().AddForce(knockbackDirection * 300f);
            GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerEnergy>().GainEnergy(5);
        }

            


        if (health <= 0)
        {
            death();

        }
        
    }

    private void attackRoutine()
    {
        attack.triggerAttack();
    }

    public void death()
    {
        alive = false;
        Destroy(agent, 1);
    }
}
