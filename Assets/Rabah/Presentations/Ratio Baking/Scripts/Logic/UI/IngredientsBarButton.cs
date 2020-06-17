using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsBarButton : MonoBehaviour
{
    [SerializeField] IngredientType ingredientBarButtonType;

    public IngredientType IngredientBarButtonType { get => ingredientBarButtonType; set => ingredientBarButtonType = value; }
}
