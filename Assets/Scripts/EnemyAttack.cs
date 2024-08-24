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
    private float timer;

    private float YWarnR = 255f;
    private float YWarnG = 216f;
    private float YWarnB = 0f;

    private float RWarnR = 255f;
    private float RWarnG = 0f;
    private float RWarnB = 0f;


    // Start is called before the first frame update
    void Start()
    {
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void Parried()
    {
        StopCoroutine(attackCheck);
        enemyMovement.isParryable = false;
        enemyMovement.canMove = true;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
    }

    public void triggerAttack()
    {
        if (timer > attackCD)
        {
            enemyMovement.canMove = false;
            timer = 0;
            Debug.Log(Random.Range(0f, 2f));
            if (Random.Range(0f, 2f) >= 0.5)
            {
                enemyMovement.isParryable = true;
                attackCheck = StartCoroutine(YellowAttackCoroutine());
            }
            else { attackCheck = StartCoroutine(RedAttackCoroutine()); }

            }

        
    }

    public IEnumerator RedAttackCoroutine()
    {
        warningLight.color = new Color(RWarnR, RWarnG, RWarnB, 1);

        yield return new WaitForSeconds(0.3f);
        enemyMovement.isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        enemyMovement.isAttacking = false;
        enemyMovement.canMove = true;

    }

    public IEnumerator YellowAttackCoroutine()
    {
        warningLight.color = new Color(YWarnR, YWarnG, YWarnB, 1);

        yield return new WaitForSeconds(0.3f);
        enemyMovement.isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        enemyMovement.isParryable = false;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        enemyMovement.isAttacking = false;
        enemyMovement.canMove = true;

    }
}
