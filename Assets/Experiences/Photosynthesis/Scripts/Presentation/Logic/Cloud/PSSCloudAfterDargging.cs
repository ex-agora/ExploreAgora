using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSCloudAfterDargging : MonoBehaviour
{
    [SerializeField] RainTextureHandler rain;
    [SerializeField] GameObject cloudSystem;
    public void PlayRainAnim ()
    {
        print ("Raining Raining Raining Raining Raining Raining Raining ");
        rain.StartRain ();
        Invoke (nameof(EndRaining) , 3);
    }
    public void ResetCloudPosition ()
    {
        transform.position = GetComponent<Draggable> ().MyPosition;
    }
    void EndRaining ()
    {
        rain.StopRain ();
        cloudSystem.SetActive (false);
    }
}
