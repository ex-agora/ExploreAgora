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
    //[SerializeField] List<GameObject> burger;
    //[SerializeField] List<GameObject> bread;
    //[SerializeField] List<GameObject> cheese;
    //[SerializeField] List<GameObject> extras;


    [SerializeField] GameObject currentHolder, nextHolder, previousHolder;





    void FadeInOutHolders()
    {
        //fade out last holder components 


        Debug.Log("fade in current");

        currentHolder.SetActive(true);

        if (previousHolder != null)
            previousHolder.SetActive(false);
     


        if (nextHolder != null)
            nextHolder.SetActive(false);
      


    
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
            for (int k = 0; k < BreadToBeEnabled.Length; k++)
                breadHolder.transform.GetChild(BreadToBeEnabled[k]).GetComponent<ModelsHandlers>().ModelPanelsHandlers();

            for (int i = 0; i < CheeseToBeEnabled.Length; i++)
                cheeseHolder.transform.GetChild(CheeseToBeEnabled[i]).GetComponent<ModelsHandlers>().ModelPanelsHandlers();

            for (int k = 0; k < BreadToBeDisabled.Length; k++)
                breadHolder.transform.GetChild(BreadToBeDisabled[k]).GetComponent<ModelsHandlers>().ModelColliders();

            for (int i = 0; i < CheeseToBeDisabled.Length; i++)
                cheeseHolder.transform.GetChild(CheeseToBeDisabled[i]).GetComponent<ModelsHandlers>().ModelColliders();
        }
    }

}





