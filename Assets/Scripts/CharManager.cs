using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    [SerializeField] private GameObject Char1;
    [SerializeField] private GameObject Char2;
    [SerializeField] private CinemachineFreeLook virCamera;
    public GameObject activeChar;
    private float timer;

    void Start()
    {
        activeChar = Char1;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void SwapNextChar()
    {
        if (activeChar == Char1 && timer <= 0)
        {
            StartCoroutine(SetActive(Char2));
            timer = 2f;

        }
        else if (activeChar == Char2 && timer <= 0)
        {
            StartCoroutine(SetActive(Char1));
            timer = 2f;
        }
    }

    public IEnumerator SetActive(GameObject targetChar)
    {

        targetChar.transform.position = activeChar.transform.position + virCamera.GetComponent<ThirdPersonCam>().orientation.right * 1.4f;
        targetChar.SetActive(true);
        targetChar.GetComponent<InputManager>().enabled = true;
        activeChar.GetComponent<InputManager>().enabled = false;
        virCamera.m_Follow = targetChar.transform.Find("PlayerObj");
        virCamera.m_LookAt = targetChar.transform.Find("PlayerObj");
        yield return new WaitForSeconds(0.1f);
        targetChar.GetComponent<PlayerAttack>().ParryCheck();
        yield return new WaitForSeconds(1.3f);
        activeChar.SetActive(false);
        activeChar = targetChar;
    }
}
