using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MothScoreHandler : MonoBehaviour
{
    [SerializeField] SummaryHandler summaryHandler;
    [SerializeField] Text tittle;
    [SerializeField] Text blackScore;
    [SerializeField] Text whiteScore;
    [SerializeField] Text note;
    [SerializeField] Text buttonText;
    [SerializeField] GameEvent continueFlow;
    [SerializeField] GameEvent repeat;

    bool isRepeating;

    public GameEvent ContinueFlow { get => continueFlow; set => continueFlow = value; }
    public GameEvent Repeat { get => repeat; set => repeat = value; }

    public void ShowSummary(bool _isRepeating, string _tittle,string _buttonText ,string _blackScore, string _whiteScore, string _note)
    {
        isRepeating = _isRepeating;

        tittle.text = _tittle;
        blackScore.text = _blackScore;
        whiteScore.text = _whiteScore;
        note.text = _note;
        buttonText.text = _buttonText;

        summaryHandler.ViewSummary();
    }

    public void Confirmation()
    {
        if (isRepeating)
        {
            Repeat.Raise();
        }else
        {
            ContinueFlow.Raise();
        }
    }
}
