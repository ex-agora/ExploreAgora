using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class attache to each model and hold compontents related to it.
/// Called in statisticalChartsManager to use this components
/// </summary>
public class ChartObject : MonoBehaviour
{
    public List<GameObject> Sugars = new List<GameObject>();
    public Text Result;
    public GraphSlider SliderValue;
    public GameObject Bar;
    public GameObject ValueBackground;
    public Animator LabelAnim;
    public Animator InfoAnim;
    public Animator SliderValueAnim;
}