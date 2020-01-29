using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDefenceManager : MonoBehaviour
{
    static PlantDefenceManager instance;
    int hotSpotIndex;
    bool isDefaultShowed = false;
    public static PlantDefenceManager Instance { get => instance; set => instance = value; }
    public PlantDefenceData Data { get => data; set => data = value; }
    public List<PlantDefenceElement> PlantElements { get => plantElements; set => plantElements = value; }

    List<PlantDefenceElement> plantElements;
    [SerializeField]
    List<ClickHandler> plantElementsClickables;
    [SerializeField] PlantDefenceElement defaultElement;
    [SerializeField] PlantDefenceData data;
    private void Awake ()
    {
        instance = this;
    }
    private void Start ()
    {
        PlantElements = Data.plantElements;
    }
    void ShowDefaultHotspot ()
    {
        print ("ShowDefaultHotspot");
        defaultElement.Hotspot.ShowHotspot ();
        foreach ( var element in PlantElements )
        {
            if ( element == defaultElement )
            {
                PlantElements.Remove (element);
                break;
            }
        }
        isDefaultShowed = true;
    }
    void ShowRandomHotspot ()
    {
        if ( PlantElements.Count > 0 )
        {
            hotSpotIndex = Random.Range (0 , PlantElements.Count - 1);
            PlantElements [hotSpotIndex].Hotspot.ShowHotspot ();
            print ("ShowRandomHotspot" + PlantElements [hotSpotIndex].name);
            PlantElements.Remove (PlantElements [hotSpotIndex]);
        }
    }
    public void ShowAllHotspots ()
    {
        foreach ( var element in PlantElements )
        {
            element.Hotspot.ShowHotspot ();
        }
        PlantElements.Clear ();
    }
    public void HideHotspot (PlantDefenceElement element)
    {
        PlantElements.Remove (element);
        element.Hotspot.HideHotspot ();
    }
    public void ShowHotspot ()
    {
        ShowRandomHotspot ();
        DisableAllElementsClick ();
        //if (!isDefaultShowed)
        //{
        //    ShowDefaultHotspot ();
        //}else
        //{
        //    ShowRandomHotspot ();
        //}
    }
    public void EnableAllElementsClick ()
    {
        for ( int i = 0 ; i < plantElementsClickables.Count ; i++ )
        {
            plantElementsClickables [i].enabled = true;
        }
        for ( int i = 0 ; i < plantElements.Count ; i++ )
        {
            plantElements [i].Hotspot.EnableHotspotClick();
        }
    }
    public void DisableAllElementsClick ()
    {
        for ( int i = 0 ; i < plantElementsClickables.Count ; i++ )
        {
            plantElementsClickables [i].enabled = false;
        }
        for ( int i = 0 ; i < plantElements.Count ; i++ )
        {
            plantElements[i].Hotspot.DisableHotspotClick ();
        }
    }
    /*public void RestartTimer()
    {
        PlantDefenceGameManager.Instance.WaitingForTab();
        print("WaitingForTab WaitingForTab WaitingForTab WaitingForTab");
    }*/
}
