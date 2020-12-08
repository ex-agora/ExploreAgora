using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GraphSlider : MonoBehaviour
{
    public Transform SliderOffsetParent;
    //public TMP_Text SliderValueText;
    public Text SliderValueText;
    public Transform Target;
    public GameObject Slider3DBar;
    [HideInInspector]
    public float SliderCheckResult;
    private Slider slider;

    public Slider Slider { get => slider; set => slider = value; }

    private void Start()
    {
        Slider = GetComponent<Slider>();
    }

    public void SetSliderValue()
    {
        Slider3DBar.transform.localScale = new Vector3(Slider3DBar.transform.localScale.x, Slider.value / 2, Slider3DBar.transform.localScale.z);
        float BarTopPosition = Target.GetComponent<MeshRenderer>().bounds.extents.y;
        SliderOffsetParent.position = new Vector3(SliderOffsetParent.position.x, Target.position.y + BarTopPosition, SliderOffsetParent.position.z);

        AudioManager.Instance?.Play("barTik", "Activity");

        SliderCheckResult = Slider.value * 5;
        SliderValueText.text = (Slider.value * 5).ToString();
    }
}