using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    [SerializeField] private GameObject Char1;
    [SerializeField] private GameObject Char2;
    [SerializeField] private CinemachineFreeLook virCamera;
    [SerializeField] private GameObject Player1HUD;
    [SerializeField] private GameObject Player2HUD;
    public GameObject activeChar;
    public GameObject activeHUD;
    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void SwapNextChar()
    {
        if (activeChar == Char1 && timer <= 0)
        {
            StartCoroutine(SetActive(Char2, Player2HUD));
            timer = 1.4f;

        }
        else if (activeChar == Char2 && timer <= 0)
        {
            StartCoroutine(SetActive(Char1, Player1HUD));
            timer = 1.4f;
        }
    }

    public IEnumerator SetActive(GameObject targetChar, GameObject targetHUD)
    {
        targetChar.transform.position = activeChar.transform.position + virCamera.GetComponent<ThirdPersonCam>().orientation.right * 1.4f;
        targetChar.SetActive(true);
        targetHUD.GetComponent<Canvas>().enabled = true;
        activeHUD.GetComponent<Canvas>().enabled = false;
        targetChar.GetComponent<InputManager>().enabled = true;
        activeChar.GetComponent<InputManager>().enabled = false;
        virCamera.m_Follow = targetChar.transform.Find("PlayerObj");
        virCamera.m_LookAt = targetChar.transform.Find("PlayerObj");
        yield return new WaitForSeconds(0.1f);
        targetChar.GetComponent<PlayerAttack>().ParryCheck();
        yield return new WaitForSeconds(1.3f);
        activeChar.SetActive(false);
        activeHUD = targetHUD;
        activeChar = targetChar;
    }

}
