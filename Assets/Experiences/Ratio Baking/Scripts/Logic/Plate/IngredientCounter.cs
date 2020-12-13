using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientCounter : MonoBehaviour
{
    int counter = 0;
    [SerializeField] Text counterText;
    IngredientType type;

    public IngredientType Type { get => type; set => type = value; }
    public int Counter { get => counter; set => counter = value; }

    public void ResetCounter()
    {
        Counter = 0;
        counterText.text = Counter + "";
    }
    public void AddCounter(Sprite counterImg)
    {
        if (Counter < 99)
        {
            if (Counter == 0) UpdateCounterImg(counterImg);
            Counter++;
            counterText.text = Counter.ToString();
        }
    }
    public void UpdateCounterImg(Sprite counterImg)
    {
        GetComponent<Image>().sprite = counterImg;
        counterText.text = string.Empty;
    }
}
