using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelBehaviour : MonoBehaviour
{
    public void PanelAction(bool answer)
    {
        if (answer)
            SceneManager.LoadScene("AnotherScene");
        else
            SceneManager.LoadScene("first Scene");
    }
}
