using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyAttack : MonoBehaviour
{
    public Material warningLight;
    public EnemyMovement enemyMovement;
    public float attackCD;
    private Coroutine attackCheck;
    private PlayerParry playerParry;
    public float timer;

    private float YWarnR = 255f;
    private float YWarnG = 216f;
    private float YWarnB = 0f;

    private float RWarnR = 255f;
    private float RWarnG = 0f;
    private float RWarnB = 0f;


    // Start is called before the first frame update
    void Start()
    {
        playerParry = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerParry>();
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "PlayerObj")
        {
            GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    public void Parried()
    {
        StopCoroutine(attackCheck);
        enemyMovement.isParryable = false;
        enemyMovement.canMove = true;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
    }

    public void SpecialAttacked()
    {
        if (enemyMovement.isAttacking) { StopCoroutine(attackCheck); }
        enemyMovement.isParryable = false;
        enemyMovement.isPerfectDodge = false;
        enemyMovement.canMove = false;
        enemyMovement.isAttacking = false;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void triggerAttack()
    {
        if (timer > attackCD)
        {
            enemyMovement.canMove = false;
            timer = 0;
            if (playerParry.parryCount > 0)
            {
                enemyMovement.isParryable = true;
                attackCheck = StartCoroutine(YellowAttackCoroutine());
            }
            else { attackCheck = StartCoroutine(RedAttackCoroutine()); }

            }

        
    }

    public IEnumerator RedAttackCoroutine()
    {
        enemyMovement.isAttacking = true;
        warningLight.color = new Color(RWarnR, RWarnG, RWarnB, 1);

        yield return new WaitForSeconds(0.3f);
        enemyMovement.isPerfectDodge = true;
        yield return new WaitForSeconds(0.2f);
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        enemyMovement.isPerfectDodge = false;
        enemyMovement.isAttacking = false;
        enemyMovement.canMove = true;

    }

    public IEnumerator YellowAttackCoroutine()
    {
        enemyMovement.isAttacking = true;
        warningLight.color = new Color(YWarnR, YWarnG, YWarnB, 1);

        yield return new WaitForSeconds(0.3f);
        enemyMovement.isPerfectDodge = true;
        yield return new WaitForSeconds(0.2f);
        enemyMovement.isParryable = false;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        enemyMovement.isPerfectDodge = false;
        enemyMovement.isAttacking = false;
        enemyMovement.canMove = true;

    }
}
