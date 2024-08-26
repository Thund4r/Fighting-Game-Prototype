using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttack : MonoBehaviour
{
    private bool faceEnemy;
    public Animator mAnimator;
    private int noOfClicks = 0;

    float lastClickedTime = 0f;
    float maxComboDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = mAnimator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
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
        if (playerHUD.GetComponent<PlayerEnergy>().energy >= 40 && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !mAnimator.GetBool("SpecialAttack")) {
            Debug.Log(playerHUD.GetComponent<PlayerEnergy>().energy);
            playerHUD.GetComponent<PlayerEnergy>().energy -= 40;
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;
            Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * 1f, 4f);
            foreach (Collider hit in colliders)
            {
                Debug.Log(hit.name);

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
        }
        else 
        {
            GetComponent<InputManager>().ToggleMove(false);
            mAnimator.SetTrigger("SpecialAttack");
            yield return new WaitForSeconds(1.54f);
            GetComponent<InputManager>().ToggleMove(true);
        }
        
        EndFaceEnemy();
    }

    public void ParryCheck()
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

        enemyMovement.attack.Parried();
        closestEnemy.gameObject.GetComponent<Animator>().SetTrigger("Parry");
        mAnimator.SetTrigger("Parry");
        yield return new WaitForSeconds(0.1f);
        enemyMovement.agent.GetComponent<Rigidbody>().AddForce(-direction * 1500f);
        yield return new WaitForSeconds(0.3f);
        EndFaceEnemy();
    }
}
