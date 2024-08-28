using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttack : MonoBehaviour
{
    private bool faceEnemy;
    private bool isExSpecial = false;
    public bool holdAttacking = false;
    public Animator mAnimator;
    private int noOfClicks = 0;
    public float energyCost;
    public float overheatCost;
    private float timer;
    private PlayerParry playerParry;
    public WeaponCollision weaponCollision;
    private GameObject chainEnemy;

    float lastClickedTime = 0f;
    float maxComboDelay = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = mAnimator.GetComponent<Animator>();
        playerParry = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerParry>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        timer -= Time.deltaTime;
    }

    void LateUpdate()
    {
        if (faceEnemy)
        {
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider hit in colliders)
            {

                if (hit.name == "EnemyObj")
                {
                    Vector3 distance = hit.transform.position - transform.position;
                    float distanceSq = distance.sqrMagnitude;
                    if (distanceSq < closestDistance)
                    {
                        closestDistance = distanceSq;
                        closestEnemy = hit.transform;
                    }

                }
            }
            if (closestEnemy != null)
            {
                Vector3 direction = (closestEnemy.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                this.transform.rotation = lookRotation;
            }
        }
    }

    public void Attack()
    {
        if (timer < 0) 
        { 
            lastClickedTime = Time.time;
            noOfClicks++;
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
            if (noOfClicks == 1)
            {
                mAnimator.SetTrigger("Attack1");
            }
            if (noOfClicks >= 2 && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
            {
                mAnimator.SetTrigger("Attack2");
            }
            if (noOfClicks >= 3 && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
            {
                mAnimator.SetTrigger("Attack3");
                noOfClicks = 0;
                timer = 0.5f;
            }
        }

}

    public void HoldAttack(bool value)
    {

        PlayerOverheat playerOverheat = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerOverheat>();
        if (playerOverheat.overheat + playerOverheat.overheatRate <= playerOverheat.maxOverheat)
        {
            holdAttacking = value;
            mAnimator.SetBool("HoldAttack", value);
        }
        else
        {
            holdAttacking = false;
            mAnimator.SetBool("HoldAttack", false);
        }
        
    }

    public void BeginFaceEnemy()
    {
        faceEnemy = true;
        GetComponent<InputManager>().ToggleMove(false);
    }

    public void EndFaceEnemy()
    {
        faceEnemy = false;
        GetComponent<InputManager>().ToggleMove(true);
    }

    public void ExSpecialCheck(GameObject playerHUD)
    {

        if (playerHUD.GetComponent<PlayerEnergy>().energy >= energyCost && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isExSpecial) {
            isExSpecial = true;
            playerHUD.GetComponent<PlayerEnergy>().LoseEnergy(energyCost);
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * 1f, 4f);
            foreach (Collider hit in colliders)
            {

                if (hit.name == "EnemyObj")
                {
                    Vector3 distance = hit.transform.position - transform.position;
                    float distanceSq = distance.sqrMagnitude;
                    if (distanceSq < closestDistance)
                    {
                        closestDistance = distanceSq;
                        closestEnemy = hit.transform;
                    }
                }
            }
            if (closestEnemy != null)
            {
                FaceEnemy(closestEnemy);
            
            }
            StartCoroutine(ExSpecialAnimation(closestEnemy));    
        }
    }

    private IEnumerator ExSpecialAnimation(Transform closestEnemy)
    {
        if (closestEnemy != null)
        {
            GetComponent<InputManager>().ToggleMove(false);
            EnemyMovement enemyMovement = closestEnemy.gameObject.GetComponent<EnemyMovement>();
            Vector3 direction = (transform.position - closestEnemy.position).normalized;
            transform.position = closestEnemy.position + direction * 3.6f;
            enemyMovement.canMove = false;
            enemyMovement.attack.SpecialAttacked();
            
            closestEnemy.gameObject.GetComponent<Animator>().SetTrigger("SpecialAttack");
            mAnimator.SetTrigger("SpecialAttack");
            yield return new WaitForSeconds(1.54f);
            GetComponent<InputManager>().ToggleMove(true);
            enemyMovement.canMove = true;
            isExSpecial = false;
        }
        else 
        {
            GetComponent<InputManager>().ToggleMove(false);
            mAnimator.SetTrigger("SpecialAttack");
            yield return new WaitForSeconds(1.54f);
            GetComponent<InputManager>().ToggleMove(true);
            isExSpecial = false;
        }
        
        EndFaceEnemy();
    }

    public void ChainAttack(GameObject enemy)
    {
        chainEnemy = enemy;
        GetComponent<InputManager>().EnableChainAttack();

        Time.timeScale = 0.01f;
    }

    public IEnumerator TriggerChainAttack()
    {

        GetComponent<InputManager>().DisableChainAttack();
        playerParry.GainParry(2);
        Time.timeScale = 0.5f;
        EnemyMovement enemyMovement = chainEnemy.gameObject.GetComponent<EnemyMovement>();
        Vector3 direction = (transform.position - chainEnemy.transform.position).normalized;
        transform.position = chainEnemy.transform.position + direction * 3.6f;


        mAnimator.SetTrigger("ChainAttack");
        yield return new WaitForSeconds(0.6f);
        Time.timeScale = 1f;
        

    }

    public void ParryCheck()
    {
        if (playerParry.parryCount != 0) 
        { 
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
            foreach (Collider hit in colliders)
            {

                if (hit.name == "EnemyObj")
                {
                    if (hit.GetComponent<EnemyMovement>().isParryable)
                    {
                        Vector3 distance = hit.transform.position - transform.position;
                        float distanceSq = distance.sqrMagnitude;
                        if (distanceSq < closestDistance)
                        {
                            closestDistance = distanceSq;
                            closestEnemy = hit.transform;
                        }
                    }
                }
            }
            if (closestEnemy != null)
            {
                FaceEnemy(closestEnemy);
                StartCoroutine(ParryAnimation(closestEnemy));
            }
        }
    }

    private void FaceEnemy(Transform enemy)
    {
        GetComponent<InputManager>().ToggleMove(false);
        Vector3 direction = (enemy.position - transform.position).normalized;
        direction.y = 0; // Ensure the player stays level on the ground
        transform.rotation = Quaternion.LookRotation(direction);
        enemy.gameObject.GetComponent<EnemyMovement>().agent.transform.rotation = Quaternion.LookRotation(-direction);
    }
    private IEnumerator ParryAnimation(Transform closestEnemy)
    {
        EnemyMovement enemyMovement = closestEnemy.gameObject.GetComponent<EnemyMovement>();
        Vector3 direction = (transform.position - closestEnemy.position).normalized;
        transform.position = closestEnemy.position + direction * 1.4f;
        Time.timeScale = 0.7f;
        playerParry.LoseParry();
        enemyMovement.attack.Parried();
        enemyMovement.TakeDaze(25);
        closestEnemy.gameObject.GetComponent<Animator>().SetTrigger("Parry");
        mAnimator.SetTrigger("Parry");
        
        yield return new WaitForSeconds(0.1f);
        enemyMovement.agent.GetComponent<Rigidbody>().AddForce(-direction * 1500f);
        yield return new WaitForSeconds(0.3f);

        Time.timeScale = 1f;
        EndFaceEnemy();
    }

    public void SetComboFinisher(bool value)
    {
        weaponCollision.comboFinisher = value;
    }
}
