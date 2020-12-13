using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientLabelText : MonoBehaviour
{
    [SerializeField] Text labelText;
    [SerializeField] List<Transform> positions;
    [SerializeField] Animator labelAnimator;
    //int positionsCounter;
    string ingredientText;
    private void UpdateLabelText()
    {

        labelText.text = ingredientText;
        if (/*positionsCounter*/ M35Manager.Instance.PlateCounter < 3)
        {
            transform.position = positions[/*positionsCounter*/ M35Manager.Instance.PlateCounter].position;
            /*positionsCounter++;*/
        }
    }
    public void UpdateLabel(string text)
    {
        if (/*positionsCounter*/ M35Manager.Instance.PlateCounter > 0)
            HideLabel();
        ingredientText = text;
        Invoke(nameof(UpdateLabelText), 0.5f);
        if (/*positionsCounter*/ M35Manager.Instance.PlateCounter < 3)
            Invoke(nameof(ShowLabel), 0.5f);
    }
    public void HideLabel()
    {
        labelAnimator.SetBool("fadeIn", false);
        labelAnimator.SetBool("fadeOut", true);
    }
    public void ShowLabel()
    {
        labelAnimator.SetBool("fadeOut", false);
        labelAnimator.SetBool("fadeIn", true);
    }
}
