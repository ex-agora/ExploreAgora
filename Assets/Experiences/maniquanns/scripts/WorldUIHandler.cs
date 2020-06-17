using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorldUIHandler : MonoBehaviour
{

    [SerializeField] List<Sprite> upperParts;
    [SerializeField] List<Sprite> middleParts;
    [SerializeField] List<Sprite> lowerParts;
    [SerializeField] List<Image> patterns;
    [SerializeField] List<Sprite> checkButtonSprites;
    [SerializeField] TMP_Text counter, upperText, middleText, lowerText , upperPanelText;
    [SerializeField] Image CheckButton;

    static WorldUIHandler instance;
    public static WorldUIHandler Instance { get => instance; set => instance = value; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateCounter(int score)
    {
        counter.text = score.ToString();
    }

    public void CheckButtonHandler(bool state) {
        if (state) {
        CheckButton.sprite = checkButtonSprites[1];
            CheckButton.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            CheckButton.sprite = checkButtonSprites[0];
            CheckButton.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void ChangePatterns(EContinents continents)
    {
        if(continents == EContinents.Asia)
        {
            setPatternIndex(0);
            SetPatternsText("Vietnam", "China", "Indonesia");
            upperPanelText.text = "Asia";
        }
        if(continents == EContinents.Africa)
        {
            setPatternIndex(1);
            SetPatternsText("Kenya", "Ghana", "MorocCo");
            upperPanelText.text = "Africa";
        }
        if(continents == EContinents.SouthAmerica)
        {
            setPatternIndex(2);
            SetPatternsText("Jamaica", "Chile", "USA");
            upperPanelText.text = "America";
        }
    }

    void setPatternIndex(int i)
    {
        patterns[0].sprite = upperParts[i];
        patterns[1].sprite = middleParts[i];
        patterns[2].sprite = lowerParts[i];
    }
    void SetPatternsText(string upper , string middle , string lower)
    {
        upperText.text = upper;
        middleText.text = middle;
        lowerText.text = lower;
    }
}
