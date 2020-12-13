using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M35PlateRatioBaking : MonoBehaviour
{
    [SerializeField] RatioBakingPlateStates plateState;
    [SerializeField] List<Ingredient> ingredients;
    [SerializeField] IngrdientComponent ingrdientComponent;
    [SerializeField] IngredientCounterHandler ingredientCounterHandler;
    [SerializeField] List<int> ingredientsQuantity;
    [SerializeField] [TextArea] string ingrdientLabelData;
    [SerializeField] FadeInOut result;
    [SerializeField] ParticleSystem vfx;
    List<float> answerRatios;

    public List<Ingredient> Ingredients { get => ingredients; set => ingredients = value; }
    public IngrdientComponent IngrdientComponent { get => ingrdientComponent; set => ingrdientComponent = value; }
    public List<int> IngredientsQuantity { get => ingredientsQuantity; set => ingredientsQuantity = value; }
    public FadeInOut Result { get => result; set => result = value; }

    //public IngredientCounterHandler IngredientCounterHandler { get => ingredientCounterHandler; set => ingredientCounterHandler = value; }

    private void Start()
    {
        answerRatios = new List<float>();
    }
    public void UnlockPlate()
    {
        plateState = RatioBakingPlateStates.Unlocked;
        M35Manager.Instance.IngrdientLabelText.UpdateLabel(ingrdientLabelData);
        ingredientCounterHandler.ShowComponentWithFade(ingrdientComponent , ingredients);
        M35GameManager.Instance.CurrentIngredientTypes.Clear();
    }
    public void HideCounter() => ingredientCounterHandler.HideLabel();
    public void ReloadPlate()
    {
        plateState = RatioBakingPlateStates.Unlocked;
        ingredientCounterHandler.ShowComponent(ingrdientComponent, ingredients);
        M35GameManager.Instance.CurrentIngredientTypes.Clear();
    }
    public List<float> GetAnswerRatios()
    {
        if (ingredientsQuantity.Count == 2)
        {
            answerRatios.Add(M35Manager.Instance.GetRaioIngredients(ingredientsQuantity[0], ingredientsQuantity[1]));
        }
        else if (ingredientsQuantity.Count == 3)
        {
            answerRatios = M35Manager.Instance.GetRaioIngredients(ingredientsQuantity[0], ingredientsQuantity[1], ingredientsQuantity[2]);
        }
        return answerRatios;
    }
    public void ShowResult() {
        result.gameObject.SetActive(true);
        result.fadeInOut(true);
        vfx.Play(true);
        AudioManager.Instance?.Play("revealObject", "Activity");
    }
}
