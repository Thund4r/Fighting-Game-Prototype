using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public GameObject HitVFX;
    [SerializeField] private Animator mAnimator;
    [SerializeField] private SwordEnergy swordEnergy;
    public bool comboFinisher = false;
    public bool swordFinisher = false;
    

    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "EnemyObj")
        {
            collision.GetComponent<EnemyMovement>().TakeDamage(1);
            collision.GetComponent<EnemyMovement>().TakeDaze(1);
            GameObject HitVFXObj = Instantiate(HitVFX, collision.ClosestPoint(transform.position), Quaternion.identity);

            Destroy(HitVFXObj, 0.5f);


            if (comboFinisher && !collision.GetComponent<EnemyMovement>().isChained)
            {
                GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerAttack>().ChainAttack(collision.gameObject);
            }

            if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Chain Attack"))
            {
                collision.GetComponent<EnemyMovement>().isChained = true;
            }

            if (swordFinisher)
            {
                swordEnergy.GainLevel();
                swordEnergy.energy = 0;
                swordFinisher = false;
            }
            
        }
    }
}
