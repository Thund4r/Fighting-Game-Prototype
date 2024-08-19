using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "EnemyObj")
        {
            collision.GetComponent<EnemyMovement>().TakeDamage(1);
        }
    }
}
