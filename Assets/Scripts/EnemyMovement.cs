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
    [SerializeField] private CharManager charManager;
    private GameObject player;
    private float distance;  
    private bool alive = true;
    [SerializeField] private AudioClip[] damageSFX;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHP;
        player = charManager.activeChar;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        player = charManager.activeChar;
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

    public void Lunge()
    {

        agent.GetComponent<Rigidbody>().AddForce(agent.transform.forward * (movementSpeed * 500));
    }

    public void LookAtPlayer()
    {
        agent.transform.LookAt(charManager.activeChar.transform);
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
            isPerfectDodge = false;
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
        SoundManager.instance.PlaySoundFXs(this.transform, damageSFX);
        if (canMove || isAttacking)
        {
            this.attack.timer = Mathf.Clamp(this.attack.timer - (this.attack.timer * (health/(maxHP*8)) + 0.07f), 0, 1000);
            Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (charManager.activeChar.name == "Player 1")
            {
                charManager.activeHUD.GetComponent<PlayerEnergy>().GainEnergy(5);
            }
            else if (charManager.activeChar.name == "Player 2")
            {
                charManager.activeHUD.GetComponent<SwordEnergy>().GainEnergy(5);
            }
            
            
        }

            


        if (health <= 0)
        {
            death();

        }
        
    }

    private void attackRoutine()
    {
        attack.triggerAttack("Attack1");
    }

    public void death()
    {
        alive = false;
        Destroy(agent, 1);
    }
}
