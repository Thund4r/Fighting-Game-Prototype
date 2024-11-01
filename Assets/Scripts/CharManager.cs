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

    void Start()
    {
        activeChar = Char1;
    }

    public void SwapNextChar()
    {
        if (activeChar == Char1)
        {
            SetActive(Char2);

        }
        else if (activeChar == Char2)
        {
            SetActive(Char1);
        }
    }

    public void SetActive(GameObject targetChar)
    {
        targetChar.SetActive(true);
        activeChar.SetActive(false);
        activeChar = targetChar;
        virCamera.m_Follow = activeChar.transform;
        virCamera.m_LookAt = activeChar.transform;
    }
}
