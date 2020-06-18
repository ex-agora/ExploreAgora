using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUIHandler : MonoBehaviour
{
    static ScreenUIHandler instance;
    [SerializeField] Text correctScore, wrongScore, bigScore, scoreOfPanel;
    [SerializeField] List<Image> panelStars, recapStars;
    [SerializeField] List<Sprite> stars;
    [SerializeField] ToolBarHandler scoreCounter;
    int nextScore;
    public static ScreenUIHandler Instance { get => instance; set => instance = value; }
    public ToolBarHandler ScoreCounter { get => scoreCounter; set => scoreCounter = value; }

    [SerializeField] int firstStar, secondStar, thirdStar;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        ResetUI();
    }
    private void Start()
    {
        nextScore = firstStar;
        scoreOfPanel.text = $"{0}/{nextScore}";
    }
    public bool IsPass(int counter) => counter >= firstStar;
    #region Public_Methods

    public void UpdateUIScore()
    {
        scoreOfPanel.text = $"{SandwichComponentsHandler.Instance.CorrectCounter.ToString()}/{nextScore}";
        bigScore.text = SandwichComponentsHandler.Instance.CorrectCounter.ToString();
        correctScore.text = SandwichComponentsHandler.Instance.CorrectCounter.ToString();
        wrongScore.text = SandwichComponentsHandler.Instance.WrongCounter.ToString();

        if (SandwichComponentsHandler.Instance.CorrectCounter > thirdStar)
        {
            scoreOfPanel.gameObject.SetActive(false);
            bigScore.gameObject.SetActive(true);
        }

        UpdateStars(SandwichComponentsHandler.Instance.CorrectCounter);
    }

    public void onFailure()
    {
        Debug.Log("on Failure");
        if (SandwichComponentsHandler.Instance.CorrectCounter > 4)
            bigScore.color = Color.red;

        if (SandwichComponentsHandler.Instance.CorrectCounter < 4)
            scoreOfPanel.color = Color.red;

        Invoke(nameof(ReturnColorWhite), 2);
    }

    void ReturnColorWhite()
    {
        bigScore.color = Color.white;
        scoreOfPanel.color = Color.white;
    }

    public void ResetUI()
    {
        for (int i = 0; i < panelStars.Count; i++)
        {
            panelStars[i].sprite = stars[0];
            recapStars[i].sprite = stars[0];
        }
        correctScore.text = "0";
        wrongScore.text = "0";
        bigScore.text = "0";
        nextScore = firstStar;
        scoreOfPanel.text = $"{0}/{nextScore}";
        scoreOfPanel.gameObject.SetActive(true);
        bigScore.gameObject.SetActive(false);
    }

    #endregion

    void UpdateStars(int score)
    {
        if (score > 0 && score <= firstStar)
        {
            panelStars[0].sprite = stars[1];
            recapStars[0].sprite = stars[1];
        }
        else if (score > firstStar && score <= secondStar)
        {
            nextScore = secondStar;
            panelStars[0].sprite = stars[1];
            recapStars[0].sprite = stars[1];
            panelStars[1].sprite = stars[1];
            recapStars[1].sprite = stars[1];
        }
        else if (score > secondStar && score <= thirdStar)
        {
            nextScore = thirdStar;
            panelStars[0].sprite = stars[1];
            recapStars[0].sprite = stars[1];
            panelStars[1].sprite = stars[1];
            recapStars[1].sprite = stars[1];
            panelStars[2].sprite = stars[1];
            recapStars[2].sprite = stars[1];
        }
        scoreOfPanel.text = $"{SandwichComponentsHandler.Instance.CorrectCounter.ToString()}/{nextScore}";
    }


    void ScoreAnimation()
    {
        Debug.Log("ScoreAnimation");
    }


}
