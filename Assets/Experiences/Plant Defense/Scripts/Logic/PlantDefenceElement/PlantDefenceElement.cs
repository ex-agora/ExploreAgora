using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDefenceElement : MonoBehaviour

{
    [SerializeField] PlantDefenceHotspot hotspot;
    [SerializeField] LabelWorldHandler label;
    bool isLabelOpened = false;
    bool isSheildUpdated = false;
    PDInformationPanel pDInformation;
    bool isSummaryViewed;
    public PlantDefenceHotspot Hotspot { get => hotspot; set => hotspot = value; }
    public LabelWorldHandler Label { get => label; set => label = value; }

    // Start is called before the first frame update
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }
    public void UpdateSheild ()
    {
        if ( !isSheildUpdated )
        {
            PlantDefenceGameManager.Instance.SheildCounter.UpdateSheildFrag ();
            isSheildUpdated = true;
        }
    }
    public void ToggleLabels ()
    {
        if ( isLabelOpened )
        {
            label.HidaLabel ();
            isLabelOpened = false;
        }
        else
        {
            label.ShowLabel ();
            isLabelOpened = true;
        }
    }
    public void PlayBubbleAnimator(string animator)
    {
        pDInformation = PlantDefenceGameManager.Instance.InformationPanelManager.SetAnimatorController(animator);
        Invoke(nameof(PlayBubbleAfterTime), PlantDefenceGameManager.Instance.FlowDurations.beforeSummaryTime);
    }
    void PlayBubbleAfterTime()
    {
        if (!isSummaryViewed)
        {
            PlantDefenceGameManager.Instance.BubbleAnimator.enabled = true;
            PlantDefenceGameManager.Instance.BubbleAnimator.SetInteger("panelState", pDInformation.PanelState);
            PlantDefenceManager.Instance.DisableAllElementsClick();
            PlantDefenceGameManager.Instance.MidSummary.ContentSprite = pDInformation.FirstFrame;
            PlantDefenceGameManager.Instance.MidSummary.ViewSummary();
            isSummaryViewed = true;
        }
    }
}
