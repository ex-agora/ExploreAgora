using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickActiveStateHandler : MonoBehaviour
{
    [SerializeField] List<ClickHandler> clickables;
    public void EnableClickables() {
        for (int i = 0; i < clickables.Count; i++)
        {
            clickables[i].enabled = true;
        }
    }
    public void DisableClickables() {
        for (int i = 0; i < clickables.Count; i++)
        {
            clickables[i].enabled = false;
        }
    }
}
