using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichComponentsBehavior : MonoBehaviour
{

    [SerializeField] List<GameObject> platecomponents;

    [SerializeField] GameObject lockedFlow , unLockedFlow;
    static SandwichComponentsBehavior instance;

    public static SandwichComponentsBehavior Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }


    public void BeginXperience()
    {
        if(SandwichComponentsHandler.Instance.BurgerExperineceType == EBurgerExperineceType.WithLocking)
        {
            lockedFlow.SetActive(true);
            SandwichComponentsHandler.Instance.CurrentFlow = lockedFlow.GetComponent<BurgerSandwichScenariosHandler>();          
        }
        else
        {
            unLockedFlow.SetActive(true);
            SandwichComponentsHandler.Instance.CurrentFlow = unLockedFlow.GetComponent<BurgerSandwichScenariosHandler>();
        }

        SandwichComponentsHandler.Instance.SetFlowHolders();
        SandwichComponentsHandler.Instance.SetCorrectAnswersForOrders(SandwichComponentsHandler.Instance.Orders);
    }


    public void ResetPlate()
    {
        FadeInOut[] fadeList = null;
        Debug.Log("Reset Plate");
        for (int i = 0; i < platecomponents.Count; i++)
        {
            
            fadeList = platecomponents[i].GetComponentsInChildren<FadeInOut>();
            for (int j = 0; j < fadeList.Length; j++)
            {
                fadeList[j].SetFadeAmount(0);
            }
        }
        Invoke(nameof(HidePlat), 0.7f);
    }
    void HidePlat() {
        for (int i = 0; i < platecomponents.Count; i++)
        {
            platecomponents[i].SetActive(false);
        }
    }
    public void CompleteOrderState(bool CompleteState)
    {
        var f = platecomponents[0].GetComponent<FadeInOut>();
        if (CompleteState)
        {
            platecomponents[0].SetActive(CompleteState);
            f?.SetFadeAmount(CompleteState ? 1 : 0);
        }
        else {
            f?.SetFadeAmount(CompleteState ? 1 : 0);
            platecomponents[0].SetActive(CompleteState);
        }

    }


    
}
