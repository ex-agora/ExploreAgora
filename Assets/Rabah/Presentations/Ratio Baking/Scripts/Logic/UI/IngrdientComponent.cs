using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngrdientComponent : MonoBehaviour
{
    [SerializeField] List<IngredientCounter> ingredientCounters;
    public List<IngredientCounter> IngredientCounters { get => ingredientCounters; set => ingredientCounters = value; }
    public void PrepareComponent(List<Ingredient> ingredients)
    {
        ResetCounter(ingredients);
        UpdateCounterIngredientType(ingredients);
    }
    void UpdateCounterIngredientType(List<Ingredient> ingredients)
    {
        for (int i = 0; i < IngredientCounters.Count; i++)
        {
            IngredientCounters[i].Type = ingredients[i].IngredientType;
        }
    }
    void ResetCounter(List<Ingredient> ingredients)
    {
        for (int i = 0; i < IngredientCounters.Count; i++)
        {
            IngredientCounters[i].ResetCounter();
            IngredientCounters[i].UpdateCounterImg(ingredients[i].InActiveCounterImg);
        }
    }
    public List<float> GetRatio()
    {
        List<float> ratios = new List<float>();
        List<int> counters = new List<int>();
        for (int i = 0; i < IngredientCounters.Count; i++)
        {
            counters.Add(IngredientCounters[i].Counter);
        }
        if (IngredientCounters.Count == 2) ratios.Add(M35Manager.Instance.GetRaioIngredients(counters[0], counters[1]));
        if (IngredientCounters.Count == 3) ratios = M35Manager.Instance.GetRaioIngredients(counters[0], counters[1], counters[2]);
        return ratios;
    }

}
