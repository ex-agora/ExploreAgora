using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PSSFadeNightImageToLight : MonoBehaviour
{
    [SerializeField] Image nightImage;
    [SerializeField] float maxAlfa;
    Color imgColor;


    private void Start ()
    {
        imgColor = nightImage.color;
    }
    public void StartFadeImg (float fadeRatio)
    {
        //fadeRatio = 1 - fadeRatio;
        imgColor.a = ( maxAlfa - ( maxAlfa * fadeRatio ) ) / 255;
        Debug.LogWarning (fadeRatio + " " + imgColor.a);
        //imgColor.a = Mathf.Clamp (imgColor.a , 0 , maxAlfa / 255);
        nightImage.color = imgColor;
        //print (nightImage.color);
    }
    public void ResetFadeImg ()
    {
        imgColor.a = maxAlfa / 256;
        nightImage.color = imgColor;
    }
    public void StopFadeImg ()
    {
        imgColor.a = 0;
        nightImage.color = imgColor;
    }
}
