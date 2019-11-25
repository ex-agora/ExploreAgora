using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingManagerHandler : MonoBehaviour
{
    [SerializeField] List<SnappingManager> managers;
    [SerializeField] GameEvent endQuiz;
    public void CheckAnswer() {
        bool up = true;
        for (int i = 0; i < managers.Count; i++)
        {
            up &= managers[i].IsRightAns;
        }
        if (up) {
            OnBoardingMathGameManager.Instance.score = 3;
            endQuiz?.Raise();
            for (int i = 0; i < managers.Count; i++)
            {
                managers[i].StopDrag();
            }
        }
    }
}
