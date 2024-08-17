using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Animator mAnimator;
    private int noOfClicks = 0;

    float lastClickedTime = 0f;
    float maxComboDelay = 1f;

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

    public void Attack()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        if (noOfClicks == 1)
        {
            mAnimator.SetTrigger("Attack1");
        }
        else if (noOfClicks >= 2 && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            mAnimator.SetTrigger("Attack2");
        }
        else if (noOfClicks >= 3 && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            mAnimator.SetTrigger("Attack3");
            noOfClicks = 0;
        }
        
    }

    public void BeginFaceEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in colliders)
        {

            if (hit.name == "Enemy")
            {
                Vector3 distance = hit.transform.position - player.transform.position;
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
            Vector3 direction = (closestEnemy.position - player.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            player.transform.rotation = lookRotation;
            player.GetComponent<InputManager>().ToggleMove(false);
        }

    }

    public void EndFaceEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<InputManager>().ToggleMove(true);
    }
}
