using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyAttack : MonoBehaviour
{
    public Material warningLight;
    private bool attack = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (attack == true)
        {
            if (collision.gameObject.name == "Player")
            {
                GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHealth>().TakeDamage(10);
            }
        }
        
    }

    public void triggerAttack()
    {
        /*this.gameObject.SetActive(true);
        attack = true;
        warningLight.color = new Color(warningLight.color.r, warningLight.color.g, warningLight.color.b, 1);*/

    }
}
