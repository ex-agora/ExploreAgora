using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAfterDargging : MonoBehaviour
{
    [SerializeField] Animator rainAnimator;
    public void PlayRainAnim ()
    {
        //rainAnimator.SetBool ("startRaining" , true);
        print ("Raining Raining Raining Raining Raining Raining Raining ");
    }
    public void ResetCloudPosition ()
    {
        transform.position = GetComponent<Draggable> ().MyPosition;
    }
}
