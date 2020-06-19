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
    [SerializeField] private MeshRenderer graphMesh;
    [SerializeField] private Text nameTxt;
    [SerializeField] private Image nameLabelImage;
    [SerializeField] private string nameLabel;
    [SerializeField] private int labelSize;
    [SerializeField] private bool isEquationed;
    [SerializeField] private string equationStr;
    [SerializeField] private Collider objectCollider = null;
    [SerializeField] private int indexButton;

    private bool isDragged = false;
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

        if (objectCollider != null)
            objectCollider.enabled = true;

        isDragged = true;
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
            graphMesh.material.color = DefualtColor;
    }

    public void CorrectChoice()
    {
        graphMesh.material.color = CorrectChoiceColor;
        sliderValue.Slider.interactable = false;
    }

    public void WrongChoice()
    {
        graphMesh.material.color = WrongChoiceColor;
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

    public void HideSugar()
    {
        sugarObjectController.HiddingSugar();
    }

    public void ShowResultLabel()
    {
        if (isEquationed)
            nameTxt.text = equationStr;
        else
            nameTxt.text = result.ToString();
    }

    private void OnDestroy()
    {
        StatisticalChartsObjectManager.Instance?.RemoveObjectHandler(this);

        if (!isDragged)
            DragToWorldUIHandler.Instance?.RestoreButton(indexButton);
    }
}