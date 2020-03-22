using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterButtonsBehavior : MonoBehaviour
{
    [SerializeField] GameObject instructionPanel;
    public void EnableInstructionPanel()
    {
        instructionPanel.SetActive(true);
    }
}
