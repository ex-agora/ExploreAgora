using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatisticalChartsManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] bool nextState;
    [SerializeField] MenuUIHandler menu;

    static StatisticalChartsManager instance;
    int wrongTrialCount;

    public static StatisticalChartsManager Instance { get => instance; set => instance = value; }
    public int WrongTrialCount { get => wrongTrialCount; set => wrongTrialCount = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        WrongTrialCount = 0;
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }

    public void StartFirstQuiz()
    {
        nextState = true;
    }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoTOHome()
    {
        //throw new System.NotImplementedException();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }

}
