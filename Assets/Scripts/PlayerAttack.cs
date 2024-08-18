using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        mAnimator = GetComponent<Animator>();
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

                if (hit.name == "Enemy")
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
                GetComponent<InputManager>().ToggleMove(false);
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

    }

    public void EndFaceEnemy()
    {
        faceEnemy = false;
        GetComponent<InputManager>().ToggleMove(true);
    }

    public void ParryCheck()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in colliders)
        {

            if (hit.name == "Enemy")
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
            closestEnemy.gameObject.GetComponent<Animator>().SetTrigger("Parry");
            GetComponent<Animator>().SetTrigger("Parry");
        }
    }
}
