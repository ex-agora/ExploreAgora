using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeNightImageToLight : MonoBehaviour
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
        imgColor.a = ( maxAlfa - ( maxAlfa * fadeRatio ) ) / maxAlfa;
        imgColor.a = Mathf.Clamp (imgColor.a , 0 , maxAlfa / 256);
        nightImage.color = imgColor;
    }
    public void ResetFadeImg ()
    {
        imgColor.a = maxAlfa / 256;
        nightImage.color = imgColor;
    }
}
