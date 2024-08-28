using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public GameObject HitVFX;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "EnemyObj")
        {
            collision.GetComponent<EnemyMovement>().TakeDamage(1);
            collision.GetComponent<EnemyMovement>().TakeDaze(2);
            GameObject HitVFXObj = Instantiate(HitVFX, collision.ClosestPoint(transform.position), Quaternion.identity);

            Destroy(HitVFXObj, 0.5f);
        }
    }
}
