using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDragging : MonoBehaviour
{
    [SerializeField] Transform maxPosition;
    Vector3 startPosition;
    [SerializeField] FadeNightImageToLight fadeToLight;
    float distance;
    float ratio;
    private void Start ()
    {
        startPosition = transform.position;
        distance = maxPosition.position.y - startPosition.y;
        print (maxPosition.position.y + " " + startPosition.y + " " + distance);
    }
    public void ResetSunPosition ()
    {
        transform.position = GetComponent<Draggable> ().MyPosition;
    }
    public void FadeNightImage ()
    {
        ratio = ( transform.position.y - startPosition.y ) / distance;
        fadeToLight.StartFadeImg (ratio);
    }
}
