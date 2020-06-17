using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    [SerializeField] IngredientType  ingredientType;
    [SerializeField] Sprite counterImg;
    [SerializeField] Sprite inActiveCounterImg;
    public Sprite CounterImg { get => counterImg; set => counterImg = value; }
    public Sprite InActiveCounterImg { get => inActiveCounterImg; set => inActiveCounterImg = value; }
    public IngredientType IngredientType { get => ingredientType; set => ingredientType = value; }
}
