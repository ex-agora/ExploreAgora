using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessAccountHandler : MonoBehaviour
{
    [SerializeField] private InputField usernameText;
    [SerializeField] private InputField passwordText;

    private void OnEnable()
    {
        ResetLoginPanel();
    }

    public void ResetLoginPanel()
    {
        usernameText.text = string.Empty;
        passwordText.text = string.Empty;

    }
}