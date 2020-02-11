using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSSunDraggingEvents : MonoBehaviour
{
    [SerializeField] Transform maxPosition;
    Vector3 startPosition;
    float distance;
    float ratio;
    private void Start ()
    {
        startPosition = transform.position;
        distance = Mathf.Abs (maxPosition.position.y - startPosition.y);
        print ("Sun Effect  start calculation" + maxPosition.position.y + " " + startPosition.y + " " + distance);
    }
    public void ResetSunPosition ()
    {
        transform.position = GetComponent<Draggable> ().MyPosition;
        PhotosynthesisGameManager.Instance.NightImageEffect.GetComponent<PSSFadeNightImageToLight> ().ResetFadeImg ();
    }
    public void FadeNightImage ()
    {
        //ratio = Mathf.Abs (transform.position.y - startPosition.y) / distance;
        ratio = Mathf.Abs (transform.position.y - startPosition.y) / distance;
        print ("Sun Effect " /*+ PhotosynthesisGameManager.Instance.NightImageEffect.GetComponent<UnityEngine.UI.Image> ().color.a*/ + " " + ratio);
        PhotosynthesisGameManager.Instance.NightImageEffect.GetComponent<PSSFadeNightImageToLight> ().StartFadeImg (ratio);
    }
    public void EndFadeNightImage ()
    {
        PhotosynthesisGameManager.Instance.NightImageEffect.GetComponent<PSSFadeNightImageToLight> ().StopFadeImg ();
    }
}
