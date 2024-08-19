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
            enemyMovement.isParryable = true;
            timer = 0;
            attackCheck = StartCoroutine(AttackCoroutine());
        }

        
    }

    public IEnumerator AttackCoroutine()
    {
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 1);

        yield return new WaitForSeconds(0.4f);
        enemyMovement.isParryable = false;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 0);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        enemyMovement.canMove = true;
        enemyMovement.isParryable = false;

    }
}
