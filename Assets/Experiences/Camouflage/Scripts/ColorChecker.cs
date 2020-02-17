using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChecker : MonoBehaviour
{
    [SerializeField] Color targetColor;
    [SerializeField] ColorPicker picker;
    [SerializeField] CanvasGroup group;
    Color currentColor = Color.black;
    public Color TargetColor { get => targetColor; set => targetColor = value; }
    public CanvasGroup Group { get => group; set => group = value; }
    public Color CurrentColor { get => currentColor; set => currentColor = value; }
    [SerializeField] MothColorRatioHandler mothColorRatioHandler;
    public void ChackColor() {
        picker.ShotColor(this);
    }
    public void CheckResult(float p) {
        /*
         
         Write your code here if (p> "value"){do whatever}
         
         */
        mothColorRatioHandler.MothCurrentRatio = p;
        CamouflageGameManger.Instance.CompareResult(p,currentColor);
    }
    
}
