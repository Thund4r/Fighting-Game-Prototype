/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyHitDetection : MonoBehaviour
{
    public float IFrame;
    private float timer;
    public EnemyMovement agent;
    private GameObject player;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = agent.health;
        player = agent.player;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
}
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        if (timer > IFrame)
        {
            if (collision.gameObject.name == "Weapon")
            {
                health -= 1;
                timer = 0;
                Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
                GetComponent<Rigidbody>().AddForce(knockbackDirection * 70000f);
                GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerEnergy>().GainEnergy(10);

            }

            if (health <= 0)
            {
                agent.death();

            }
        }
        if (collision.gameObject.name == "Player")
        {
            GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().TakeDamage(10);
        }


    }
}
*/