using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantDefenceHotspot : MonoBehaviour
{
    //[SerializeField] CanvasGroup hotSpotCanvasGroup;
    //[SerializeField] float duration = 0.1f;
    //float elpTime;
    //float step;
    //float updateRate = 0.5f;

    //void CustomUpdate ()
    //{
    //    hotSpotCanvasGroup.alpha += step;
    //    elpTime += updateRate;
    //    if ( elpTime >= duration )
    //    {
    //        CancelInvoke (nameof (CustomUpdate));
    //    }

    //}
    //public void ShowHotspot ()
    //{
    //    elpTime = 0;
    //    step = ( duration / hotSpotCanvasGroup.alpha ) * updateRate;
    //    InvokeRepeating (nameof (CustomUpdate) , 0 , updateRate);
    //}
    [SerializeField] GameObject hotspotImage;
    public void ShowHotspot ()
    {
        hotspotImage.SetActive (true);
    }
    public void HideHotspot ()
    {
        hotspotImage.SetActive (false);
    }
    public void EnableHotspotClick ()
    {
        hotspotImage.GetComponent<ClickHandler> ().enabled = true;
    }
    public void DisableHotspotClick ()
    {
        hotspotImage.GetComponent<ClickHandler> ().enabled = false;
    }
}
