using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsHandlers : MonoBehaviour
{
    [SerializeField] GameObject modelPanel;
    [SerializeField] GameObject lockUI;
    [SerializeField] GameObject nameTag;
    public void ModelPanelsHandlers()
    {
        Debug.Log(this.gameObject.name  +"     ss");
        modelPanel.SetActive(true);
        nameTag.SetActive(true);
        this.GetComponent<BoxCollider>().enabled = true;
    }
    public void HidePanel() {
        modelPanel.SetActive(false);
    }

    public void ModelColliders()
    {
        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void LockHandler(bool state)
    {
        lockUI.SetActive(state);
    }
}
