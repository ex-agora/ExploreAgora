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
            label.ShowLabel ();
            isLabelOpened = false;
        }
        else
        {
            label.HidaLabel ();
            isLabelOpened = true;
        }
    }
    public void StopTapHint ()
    {
        PlantDefenceGameManager.Instance.ResetTapHint ();
    }
    public void PlayBubbleAnimator (string animator)
    {
        pDInformation = PlantDefenceGameManager.Instance.InformationPanelManager.SetAnimatorController (animator);
        PlantDefenceGameManager.Instance.BubbleAnimator.enabled = false;
        PlantDefenceGameManager.Instance.BubbleAnimator.runtimeAnimatorController = pDInformation.Anim;
        PlantDefenceGameManager.Instance.MidSummary.ContentSprite = pDInformation.FirstFrame;
        PlantDefenceGameManager.Instance.MidSummary.ViewSummary ();
    }
}
