using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAfterDargging : MonoBehaviour
{
    [SerializeField] RainTextureHandler rain;
    public void PlayRainAnim ()
    {
        print ("Raining Raining Raining Raining Raining Raining Raining ");
        rain.StartRain ();
    }
    public void ResetCloudPosition ()
    {
        transform.position = GetComponent<Draggable> ().MyPosition;
    }
}
