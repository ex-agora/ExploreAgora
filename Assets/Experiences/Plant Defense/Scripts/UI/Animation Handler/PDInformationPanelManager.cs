using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDInformationPanelManager : MonoBehaviour
{

    [SerializeField] List<PDInformationPanel> informationPanels ;
    public PDInformationPanel SetAnimatorController (string panelName)
    {
        for (int i = 0; i < informationPanels.Count; i++)
        {
            if (panelName == informationPanels[i].PanelName)
            {
                return informationPanels[i];
            }
        }

        return null;
    }
}
