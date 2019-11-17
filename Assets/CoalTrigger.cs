using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalTrigger : MonoBehaviour
{
    [SerializeField] GameEvent AfterDragging;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FadeInOut>() != null)
        {
            AudioManager.Instance.Play("placeObject", "Activity");
            AfterDragging.Raise();
        }
        
    }
}
