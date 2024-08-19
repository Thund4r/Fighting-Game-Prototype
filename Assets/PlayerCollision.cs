using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "HitBox")
        {
            GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }
}
