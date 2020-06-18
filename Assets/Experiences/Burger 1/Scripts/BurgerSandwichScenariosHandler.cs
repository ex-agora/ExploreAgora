using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class BurgerSandwichScenariosHandler : MonoBehaviour
{



    [SerializeField] GameObject burgerHolder;
    [SerializeField] GameObject breadHolder;
    [SerializeField] GameObject cheeseHolder;
    [SerializeField] GameObject extrasHolder;
    FadeInOut[] fadeOutList = null;
    ToolBarHandler[] toolBarsList = null;
    //[SerializeField] List<GameObject> burger;
    //[SerializeField] List<GameObject> bread;
    //[SerializeField] List<GameObject> cheese;
    //[SerializeField] List<GameObject> extras;


    [SerializeField] GameObject currentHolder, nextHolder, previousHolder;





    void FadeInOutHolders()
    {
        if (currentHolder != null)
        {
            fadeOutList = currentHolder.GetComponentsInChildren<FadeInOut>();
            for (int i = 0; i < fadeOutList.Length; i++)
            {

                fadeOutList[i].StopAllCoroutines();
                fadeOutList[i].SetFadeAmount(0);
            }

            Debug.Log("fade in current");

            currentHolder.SetActive(true);
        }
        if (previousHolder != null)
        {
            fadeOutList = previousHolder.GetComponentsInChildren<FadeInOut>();
            for (int i = 0; i < fadeOutList.Length; i++)
            {
                if (fadeOutList[i].gameObject.activeInHierarchy)
                {
                    fadeOutList[i].StopAllCoroutines();
                    fadeOutList[i].fadeInOut(false);
                }
            }

            toolBarsList = previousHolder.GetComponentsInChildren<ToolBarHandler>();
            for (int i = 0; i < toolBarsList.Length; i++)
            {
                if (toolBarsList[i].gameObject.activeInHierarchy)
                {

                    toolBarsList[i].CloseToolBar();
                }
            }

        }


        if (nextHolder != null)
        {
            fadeOutList = nextHolder.GetComponentsInChildren<FadeInOut>();
            for (int i = 0; i < fadeOutList.Length; i++)
            {
                if (fadeOutList[i].gameObject.activeInHierarchy)
                {
                    fadeOutList[i].StopAllCoroutines();
                    fadeOutList[i].fadeInOut(false);
                }
            }
            toolBarsList = nextHolder.GetComponentsInChildren<ToolBarHandler>();
            for (int i = 0; i < toolBarsList.Length; i++)
            {
                if (toolBarsList[i].gameObject.activeInHierarchy)
                {

                    toolBarsList[i].CloseToolBar();
                }
            }
        }
        Invoke(nameof(ShowHolder), 0.5f);

        
    }
    void ShowHolder() {
        if(currentHolder !=null ){
            fadeOutList = currentHolder.GetComponentsInChildren<FadeInOut>();
            for (int i = 0; i < fadeOutList.Length; i++)
            {
                if (fadeOutList[i].gameObject.activeInHierarchy)
                {
                    fadeOutList[i].StopAllCoroutines();
                    fadeOutList[i].fadeInOut(true);
                }
            }
            toolBarsList = currentHolder.GetComponentsInChildren<ToolBarHandler>();
            for (int i = 0; i < toolBarsList.Length; i++)
            {
                if (toolBarsList[i].gameObject.activeInHierarchy)
                {

                    toolBarsList[i].OpenToolBar();
                }
            }

        }
        
        Invoke(nameof(HideHolders), 0.7f);
    }
    public void TapSound() => AudioManager.Instance?.Play("UIAction", "UI");
    void HideHolders() {
        
        previousHolder?.SetActive(false);
        nextHolder?.SetActive(false);
    }
    public void SetHolders()
    {
        switch (SandwichComponentsHandler.Instance.SandwichStages)
        {
            case ESandwichStages.Bread:
                previousHolder = extrasHolder;
                currentHolder = breadHolder;
                nextHolder = cheeseHolder;
                break;
            case ESandwichStages.Cheese:
                previousHolder = breadHolder;
                currentHolder = cheeseHolder;
                nextHolder = burgerHolder;
                break;
            case ESandwichStages.Burger:
                previousHolder = cheeseHolder;
                currentHolder = burgerHolder;
                nextHolder = extrasHolder;
                break;
            case ESandwichStages.Extras:
                previousHolder = burgerHolder;
                currentHolder = extrasHolder;
                nextHolder = breadHolder;
                break;
            case ESandwichStages.Done:
                previousHolder = extrasHolder;
                currentHolder = null;
                nextHolder = null;
                break;
        }
        FadeInOutHolders();
    }



    public void LockUnLockModels(ESandwichStages sandwichStages, int ModelToBeOpened)
    {
        if (sandwichStages == ESandwichStages.Bread)
            breadHolder.transform.GetChild(ModelToBeOpened).GetComponent<ModelsHandlers>().ModelPanelsHandlers();
        if (sandwichStages == ESandwichStages.Cheese)
            cheeseHolder.transform.GetChild(ModelToBeOpened).GetComponent<ModelsHandlers>().ModelPanelsHandlers();
    }

    public void LockUnLockBehavior(ESandwichStages[] e, int[] BreadToBeEnabled, int[] CheeseToBeEnabled , int[] BreadToBeDisabled, int[] CheeseToBeDisabled)
    {
        if (SandwichComponentsHandler.Instance.BurgerExperineceType == EBurgerExperineceType.WithLocking)
        {
            switch (SandwichComponentsHandler.Instance.Orders)
            {
                case EOrders.Regular:

                    break;
                case EOrders.Diabetic:
                        breadHolder.transform.GetComponentsInChildren<ModelsHandlers>()[1].ModelPanelsHandlers();
                        breadHolder.transform.GetComponentsInChildren<ModelsHandlers>()[1].LockHandler(false);
                    break;
                case EOrders.Cholestrol:
                    breadHolder.transform.GetComponentsInChildren<ModelsHandlers>()[1].HidePanel();
                    cheeseHolder.transform.GetComponentsInChildren<ModelsHandlers>()[1].LockHandler(false);
                    cheeseHolder.transform.GetComponentsInChildren<ModelsHandlers>()[1].ModelPanelsHandlers();
                    break;
                case EOrders.Gluten_Free:
                    cheeseHolder.transform.GetComponentsInChildren<ModelsHandlers>()[1].HidePanel();
                    breadHolder.transform.GetComponentsInChildren<ModelsHandlers>()[2].LockHandler(false);
                    breadHolder.transform.GetComponentsInChildren<ModelsHandlers>()[2].ModelPanelsHandlers();
                    break;
                case EOrders.Lactose_Intolerance:
                    breadHolder.transform.GetComponentsInChildren<ModelsHandlers>()[2].HidePanel();
                    cheeseHolder.transform.GetComponentsInChildren<ModelsHandlers>()[2].LockHandler(false);
                    cheeseHolder.transform.GetComponentsInChildren<ModelsHandlers>()[2].ModelPanelsHandlers();
                    break;
            }
            

            for (int k = 0; k < BreadToBeDisabled.Length; k++)
                breadHolder.transform.GetChild(BreadToBeDisabled[k]).GetComponent<ModelsHandlers>().ModelColliders();

            for (int i = 0; i < CheeseToBeDisabled.Length; i++)
                cheeseHolder.transform.GetChild(CheeseToBeDisabled[i]).GetComponent<ModelsHandlers>().ModelColliders();
        }
    }

}





