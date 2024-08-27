using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed;
    public GameObject agent;
    public EnemyAttack attack;
    public float aggro;
    public int maxHP;
    public int health;
    public bool isParryable = false;
    public bool canMove = true;
    public bool isAttacking = false;
    public bool isPerfectDodge = false;


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
        if (alive && canMove)
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
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (canMove || isAttacking)
        {
            Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
            agent.GetComponent<Rigidbody>().AddForce(knockbackDirection * 500f);
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
