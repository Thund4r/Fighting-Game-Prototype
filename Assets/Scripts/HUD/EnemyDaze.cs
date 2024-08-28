using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDaze : MonoBehaviour
{
    [SerializeField] private Image YellowDaze;
    [SerializeField] private EnemyMovement enemyMovement;

    // Update is called once per frame
    void Update()
    {
        UpdateDaze();
    }

    public void UpdateDaze()
    {
        float daze = enemyMovement.daze;
        float maxDaze = enemyMovement.maxDaze;
        float dazeFill = YellowDaze.fillAmount;
        float dazeFrac = daze / maxDaze;
        if (dazeFill != dazeFrac)
        {
            YellowDaze.fillAmount = dazeFrac;
        }
    }
}
