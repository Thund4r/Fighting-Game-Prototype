using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed;
    public GameObject agent;
    public EnemyAttack attack;
    public float aggro;
    public int health;
    public bool isParryable = false;

    private float IFrame;
    private float timer;
    private GameObject player;
    private float distance;  
    private bool alive = true;
    private bool canMove = true;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (alive && canMove)
        {
            agent.transform.LookAt(player.transform);
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
            distance = (player.transform.position - transform.position).sqrMagnitude;
            if (distance < aggro)
            {
                attackRoutine();
            }
        }
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (timer > IFrame)
        {
            if (collision.gameObject.name == "Weapon")
            {
                health -= 1;
                timer = 0;
                Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
                agent.GetComponent<Rigidbody>().AddForce(knockbackDirection * 700f);
                GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerEnergy>().GainEnergy(10);

            }
            
            if (health <= 0)
            {
                death();
                
            }
        }
        if (collision.gameObject.name == "Player")
        {
            GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().TakeDamage(10);
        }


    }

    private void attackRoutine()
    {
        canMove = false;
        isParryable = true;
        attack.triggerAttack();

    }
    public void death()
    {
        alive = false;
        Destroy(agent, 3);
        agent.GetComponent<Rigidbody>().freezeRotation = false;
    }
}
