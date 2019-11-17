using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoothGameManager : MonoBehaviour, ITriggable, IMenuHandler
{
    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] SpeechBubbleController bubbleController;
    private static MoothGameManager instance;
    private bool nextState;
    int blackMoothCount;
    int whiteMoothCount;


    public static MoothGameManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        Invoke(nameof(StartMachine), 2f);
    }

    private void StartMachine()
    {
        stateMachine.StartSM();
    }

    public void WhiteMoothHit()
    {
        whiteMoothCount++;
    }

    public void BlackMoothHit()
    {
        blackMoothCount++;
    }

    public bool GetTrigger()
    {
        bool up = nextState;
        nextState = false;
        return up;
    }

    public void GoToNextBubbleState()
    {
        nextState = true;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoTOHome()
    {
        //TODO 
    }
}
