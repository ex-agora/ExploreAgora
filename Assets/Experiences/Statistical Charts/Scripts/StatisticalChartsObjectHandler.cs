using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticalChartsObjectHandler : MonoBehaviour
{
    [SerializeField] private SugarObjectController sugarObjectController;
    [SerializeField] private Transform objectHolder;
    [SerializeField] private int result;
    [SerializeField] private GraphSlider sliderValue;
    [SerializeField] private GameObject slider;
    [SerializeField] private GameObject ValueBackground;
    [SerializeField] private Material mat;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Image nameLabelImage;
    [SerializeField] private string nameLabel;
    [SerializeField] private int labelSize;
    private Color defualtColor;
    private Color correctChoiceColor;
    private Color wrongChoiceColor;

    public Color DefualtColor { get => defualtColor; set => defualtColor = value; }
    public Color CorrectChoiceColor { get => correctChoiceColor; set => correctChoiceColor = value; }
    public Color WrongChoiceColor { get => wrongChoiceColor; set => wrongChoiceColor = value; }
    public int LabelSize { get => labelSize; set => labelSize = value; }

    private void Start()
    {
        StatisticalChartsObjectManager.Instance.AddObjectHandler(this);
        CloseDragging();
    }

    public void OpenDragging()
    {
        slider.SetActive(true);
        ValueBackground.SetActive(true);
        sliderValue.Slider.interactable = true;
    }

    public void CloseDragging()
    {
        slider.SetActive(false);
        ValueBackground.SetActive(false);
        Invoke(nameof(CloseSlider), 0.2f);
    }
    
    private void CloseSlider()
    {
        sliderValue.Slider.interactable = false;

    }
    public void DefualtChoice()
    {
        if (sliderValue.Slider.interactable)
            mat.color = DefualtColor;
    }

    public void CorrectChoice()
    {
        mat.color = CorrectChoiceColor;
        sliderValue.Slider.interactable = false;
    }

    public void WrongChoice()
    {
        mat.color = WrongChoiceColor;
    }

    public bool CheckResult() => sliderValue.SliderCheckResult == result;

    public void SetLabel(Sprite _label)
    {
        nameTxt.text = nameLabel;
        nameLabelImage.sprite = _label;
        nameLabelImage.SetNativeSize();
    }

    public void ShowNameLabel()
    {
        nameLabelImage.gameObject.SetActive(true);
    }

    public void ShowSugar()
    {
        sugarObjectController.ShowingSugar();
    }
}