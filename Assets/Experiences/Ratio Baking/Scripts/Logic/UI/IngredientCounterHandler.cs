using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCounterHandler : MonoBehaviour
{
    [SerializeField] Animator counterAnimator;
    IngrdientComponent currentComponent;
    List<Ingredient> currentIngredients;
    [SerializeField] List<IngrdientComponent> ingrdientComponents;
    public void HideLabel()
    {
        counterAnimator.SetBool("fadeIn", false);
        counterAnimator.SetBool("fadeOut", true);
        Invoke(nameof(DisableCounter), 0.5f);
    }
    public void ShowLabel()
    {
        if (M35Manager.Instance.PlateCounter < 3)
        {
            currentComponent.PrepareComponent(currentIngredients);
            EnableCounter();
            counterAnimator.SetBool("fadeOut", false);
            counterAnimator.SetBool("fadeIn", true);
        }
    }
    void EnableCounter()
    {
        currentComponent?.gameObject.SetActive(true);
    }
    void DisableCounter()
    {
        for (int i = 0; i < ingrdientComponents.Count; i++)
        {
            ingrdientComponents[i].gameObject.SetActive(false);
        }
    }
    public void ShowComponentWithFade(IngrdientComponent component, List<Ingredient> ingredients)
    {
        HideLabel();
        currentComponent = component;
        currentIngredients = ingredients;
        Invoke(nameof(ShowLabel), 0.5f);
    }
    public void ShowComponent(IngrdientComponent component, List<Ingredient> ingredients)
    {
        currentComponent = component;
        currentIngredients = ingredients;
        currentComponent.PrepareComponent(currentIngredients);
    }
}
