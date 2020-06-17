using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUIHandler : MonoBehaviour
{
    static ScreenUIHandler instance;
    [SerializeField] Text correctScore, wrongScore , bigScore , scoreOfPanel ;
    [SerializeField] List<GameObject> panelStars , recapStars;  
   
    public static ScreenUIHandler Instance { get => instance; set => instance = value; }



    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateUIScore(int correctScore , int wrongScore)
    {
        if (correctScore > 4)
            ScoreAnimation();

    }
     void UpdateRecapScore() {
        Debug.Log("UpdateRecapScore");
    }
     void ScoreAnimation() {
        Debug.Log("ScoreAnimation");
    }
    void UpdateStars() {
        Debug.Log("UpdateStars");
    }


    public void onFailure()
    {
        Debug.Log("on Failure");
    }

    public void ResetUI()
    {

    }
}
