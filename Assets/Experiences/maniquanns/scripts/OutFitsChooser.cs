using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutFitsChooser : MonoBehaviour
{

    [SerializeField] EOutfit outfit;
    [SerializeField]EBodyParts bodyParts;

    public void SetAnswer()
    {
        if (bodyParts == EBodyParts.UpperBody)
            BodyPartsHandler.Instance.currentAnswers[0] = outfit.ToString();
        else if (bodyParts == EBodyParts.MiddleBody)
            BodyPartsHandler.Instance.currentAnswers[1] = outfit.ToString();
        else if (bodyParts == EBodyParts.LowerBody)
            BodyPartsHandler.Instance.currentAnswers[2] = outfit.ToString();
    }
}
