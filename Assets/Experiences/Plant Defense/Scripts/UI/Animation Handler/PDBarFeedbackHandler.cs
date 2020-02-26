using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDBarFeedbackHandler : MonoBehaviour
{
    [SerializeField] Image folding;
    [SerializeField] Image spinose;
    [SerializeField] Image preickles;
    [SerializeField] Image thron;
    [SerializeField] Image fruit;
    [SerializeField] Sprite foldingSp;
    [SerializeField] Sprite spinoseSp;
    [SerializeField] Sprite preicklesSp;
    [SerializeField] Sprite thronSp;
    [SerializeField] Sprite fruitSp;
    public void ActiveIcon(string key) {
        switch (key) {
            case "Folding_Reflex": folding.sprite = foldingSp; break;
            case "Fruit": fruit.sprite = fruitSp; break;
            case "Prickle": preickles.sprite = preicklesSp; break;
            case "Spinose Leaf": spinose.sprite = spinoseSp; break;
            case "Thorn": thron.sprite = thronSp; break;
        }
    }
 
}
