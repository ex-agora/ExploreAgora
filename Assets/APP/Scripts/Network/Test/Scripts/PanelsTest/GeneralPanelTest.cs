using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPanelTest : MonoBehaviour
{
    public GeneralPanelTest nextPanel;
    public Text error;

    public void GoToNextPanel ()
    {
        nextPanel.gameObject.SetActive (true);
    }
    public void ShowErrors (string err)
    {
        error.text = err;
    }
}
