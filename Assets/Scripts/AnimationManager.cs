using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public PlayerAttack playerAttack;
    // Start is called before the first frame update
    public void BeginFaceEnemy()
    {
        playerAttack.BeginFaceEnemy();
    }

    public void EndFaceEnemy()
    {
        playerAttack.EndFaceEnemy();
    }
}
