using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalTrigger : MonoBehaviour
{
    [SerializeField] GameEvent AfterDragging;
    [SerializeField] bool isOnHit;
    bool isRightPlace;
    bool isDone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FadeInOut>() != null)
        {
            isRightPlace = true;
            if (isOnHit)
                CheckCoal();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<FadeInOut>() != null)
        {
            isRightPlace = false;
        }
    }
    public void CheckCoal() {
        if (isRightPlace&& !isDone) {
            isDone = true;
            AudioManager.Instance.Play("placeObject", "Activity");
            AfterDragging.Raise();
        }
    }
}
