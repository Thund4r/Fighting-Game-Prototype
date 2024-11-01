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
    private 

    void Start()
    {
        activeChar = Char1;
    }

    public void SwapNextChar()
    {
        if (activeChar == Char1)
        {
            StartCoroutine(SetActive(Char2));

        }
        else if (activeChar == Char2)
        {
            StartCoroutine(SetActive(Char1));
        }
    }

    public IEnumerator SetActive(GameObject targetChar)
    {

        targetChar.transform.position = activeChar.transform.position + virCamera.GetComponent<ThirdPersonCam>().orientation.right * 1.4f;
        targetChar.SetActive(true);
        targetChar.GetComponent<InputManager>().enabled = true;
        activeChar.GetComponent<InputManager>().enabled = false;
        virCamera.m_Follow = targetChar.transform;
        virCamera.m_LookAt = targetChar.transform;
        yield return new WaitForSeconds(0.1f);
        targetChar.GetComponent<PlayerAttack>().ParryCheck();
        yield return new WaitForSeconds(1f);
        activeChar.SetActive(false);
        activeChar = targetChar;
    }
}
