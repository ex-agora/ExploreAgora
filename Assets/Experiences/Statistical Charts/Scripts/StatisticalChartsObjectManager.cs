using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticalChartsObjectManager : MonoBehaviour
{
    private static StatisticalChartsObjectManager instance;

    [SerializeField] private List<StatisticalChartsObjectHandler> statisticalChartsObjects;

    [SerializeField] private Color defualtColor;
    [SerializeField] private Color correctChoiceColor;
    [SerializeField] private Color wrongChoiceColor;

    public static StatisticalChartsObjectManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddObjectHandler(StatisticalChartsObjectHandler _statisticalChartsObjectHandlers)
    {
        statisticalChartsObjects.Add(_statisticalChartsObjectHandlers);
    }

    public void CompareResult()
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

        if (finalResult)
            Debug.LogError("DONE");
        else
            Debug.LogError("NOT DONE");
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
        }

    }

    public void CloseModels()
    {
        for (int i = 0; i < statisticalChartsObjects.Count; i++)
            statisticalChartsObjects[i].CloseDragging();

    }
}