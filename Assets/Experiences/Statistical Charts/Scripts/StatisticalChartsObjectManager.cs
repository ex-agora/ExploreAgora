using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticalChartsObjectManager : MonoBehaviour
{
    private static StatisticalChartsObjectManager instance;

    [SerializeField] private List<StatisticalChartsObjectHandler> statisticalChartsObjects;
    [SerializeField] private ToolBarHandler toolBarHandler;
    [SerializeField] private Animator animator;

    [SerializeField] private List<Sprite> labelBackground;
    [SerializeField] private List<DragObjectInstantiate> dragObjectInstantiates;
    [SerializeField] private Color defualtColor;
    [SerializeField] private Color correctChoiceColor;
    [SerializeField] private Color wrongChoiceColor;

    [SerializeField] private bool isStatisticalCharts2 = false;
    private bool isSodaClicked = false;
    private bool isSecondPhase = false;

    public static StatisticalChartsObjectManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        if (isStatisticalCharts2)
            StatisticalCharts2GameManager.Instance.FirstPhase();
        else
            StatisticalChartsManager.Instance.StartFirstQuiz();
    }

    public void AddObjectHandler(StatisticalChartsObjectHandler _statisticalChartsObjectHandlers)
    {
        statisticalChartsObjects.Add(_statisticalChartsObjectHandlers);

        switch (_statisticalChartsObjectHandlers.LabelSize)
        {
            case 0:
                _statisticalChartsObjectHandlers.SetLabel(labelBackground[0]);
                break;
            case 1:
                _statisticalChartsObjectHandlers.SetLabel(labelBackground[1]);
                break;
            case 2:
                _statisticalChartsObjectHandlers.SetLabel(labelBackground[2]);
                break;
        }

        _statisticalChartsObjectHandlers.ShowNameLabel();

        if (isSecondPhase)
            _statisticalChartsObjectHandlers.ShowSugar();
    }

    public void RemoveObjectHandler(StatisticalChartsObjectHandler _statisticalChartsObjectHandlers)
    {
        if (statisticalChartsObjects.Contains(_statisticalChartsObjectHandlers))
            statisticalChartsObjects.Remove(_statisticalChartsObjectHandlers);

    }

    public bool CompareResult()
    {
        bool finalResult = true;
        bool tempValue;

        for (int i = 0; i < statisticalChartsObjects.Count; i++)
        {
            tempValue = statisticalChartsObjects[i].CheckResult();

            if (tempValue)
                statisticalChartsObjects[i].CorrectChoice();
            else
                statisticalChartsObjects[i].WrongChoice();

            finalResult &= tempValue;
        }

        return finalResult;
    }

    public void OpenModels()
    {
        for (int i = 0; i < statisticalChartsObjects.Count; i++)
        {
            statisticalChartsObjects[i].DefualtColor = defualtColor;
            statisticalChartsObjects[i].CorrectChoiceColor = correctChoiceColor;
            statisticalChartsObjects[i].WrongChoiceColor = wrongChoiceColor;

            statisticalChartsObjects[i].DefualtChoice();
            statisticalChartsObjects[i].OpenDragging();
            statisticalChartsObjects[i].ShowResultLabel();
        }

        for (int i = 0; i < dragObjectInstantiates.Count; i++)
            dragObjectInstantiates[i].UnEnableObject();
    }

    public void CloseModels()
    {
        for (int i = 0; i < statisticalChartsObjects.Count; i++)
            statisticalChartsObjects[i].CloseDragging();
    }

    private void SecondPhase()
    {
        animator.SetTrigger("IsOpen");
        StatisticalChartsManager.Instance.ShowFooterBar();
        StatisticalChartsManager.Instance.StartSecondPhase();
        isSecondPhase = true;
    }

    public void SodaClicked()
    {
        if (!isSodaClicked)
        {
            isSodaClicked = true;
            for (int i = 0; i < statisticalChartsObjects.Count; i++)
                statisticalChartsObjects[i].ShowSugar();
        }

        StatisticalChartsManager.Instance.WrongChoice();
    }

    public void MilkClicked()
    {
        if (!isSodaClicked)
        {
            for (int i = 0; i < statisticalChartsObjects.Count; i++)
                statisticalChartsObjects[i].ShowSugar();
        }
        StatisticalChartsManager.Instance.RightChoice();

        Invoke(nameof(SecondPhase), isSodaClicked ? 5f : 8f);
    }

    public void ShowOffSugar()
    {
        for (int i = 0; i < statisticalChartsObjects.Count; i++)
            statisticalChartsObjects[i].HideSugar();
    }

    public void OpenGraphPar()
    {
        toolBarHandler.OpenToolBar();
    }
}