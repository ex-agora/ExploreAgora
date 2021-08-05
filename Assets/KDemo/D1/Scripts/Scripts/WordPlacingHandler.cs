using UnityEngine;
using UnityEngine.UI;

namespace KDemo.D1.Scripts.Scripts
{
    public class WordPlacingHandler : MonoBehaviour
    {
        public bool IsEmpty { get; private set; } = true;
        private string answer;
        [SerializeField] private string correctAnswer;
        [SerializeField] private Text placeTxt;
        [SerializeField] private GameEvent @placedEvt;
        [SerializeField] private Image bg;
        [SerializeField] private Color rightColor;
        [SerializeField] private Color wrongColor;
        public void SetAnswer(string ans)
        {
            answer = ans;
            IsEmpty = false;
            placeTxt.text = answer;
            placedEvt.Raise();
            bg.color = CheckAnswer() ? rightColor : wrongColor;
        }

        public bool CheckAnswer() => string.Equals(answer, correctAnswer);
    }
}