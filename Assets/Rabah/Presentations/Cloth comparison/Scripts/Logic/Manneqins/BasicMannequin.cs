using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMannequin : MonoBehaviour
{
    [SerializeField] List<Mannequin> mannequins;
    [SerializeField] MannequinType mannequinTtype;
    [SerializeField] SS17MapHotspot hotspot;
    byte counter = 0;
    //On Click Event
    public void GetNextMannequin ()
    {
        if ( SS17GameManager.Instance.IsTutorialFinished )
        {
            mannequins [counter].gameObject.SetActive (false);
            HandleCounter ();
            if ( mannequinTtype == MannequinType.None )
                Debug.LogWarning ("Please Choose Mannquin Type");
            else if ( mannequinTtype == MannequinType.Blue )
                SS17Manager.Instance.Blue = mannequins [counter];
            else if ( mannequinTtype == MannequinType.Purple )
                SS17Manager.Instance.Purple = mannequins [counter];
            mannequins [counter].gameObject.SetActive (true);
            SS17Manager.Instance.ActiveMatchButton ();
            UpdateHotspot ();
        }
    }
    void UpdateHotspot ()
    {
        hotspot.gameObject.SetActive (true);
        hotspot.UpdatePosition (mannequins [counter].HotspotPos.position);
    }
    void HandleCounter ()
    {
        counter++;
        counter %= 3;
    }
}
