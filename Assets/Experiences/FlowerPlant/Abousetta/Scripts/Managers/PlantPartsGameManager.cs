using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class PlantPartsGameManager : MonoBehaviour
{
    [SerializeField] StateMachineManager stateMachine;
    static PlantPartsGameManager instance;
    [SerializeField] Camera arCamera;
    [SerializeField] HotSpotGroupManager[] spotGroupManagers;
    [SerializeField] CounterUIHandler uIVounterHandler;
    int expolreCount;
    int wrongTrialCount;
    public static PlantPartsGameManager Instance { get => instance; set => instance = value; }
    public Camera ArCamera { get => arCamera; set => arCamera = value; }
    public int WrongTrialCount { get => wrongTrialCount; set => wrongTrialCount = value; }

    private void Awake ()
    {
        if ( Instance == null )
            Instance = this;
        WrongTrialCount = 0;
    }
    private void Start()
    {
        Invoke(nameof(StartMachin), 2f);
        expolreCount = 0;
    }

    private void StartMachin()
    {
        stateMachine.StartSM();
    }

    public void CheckExploraState() {
        expolreCount++;
        if (expolreCount == spotGroupManagers.Length) { Debug.Log("Done"); }
        else {
            for (int i = 0; i < spotGroupManagers.Length; i++)
            {
                spotGroupManagers[i].gameObject.SetActive(true);
            }
        }
    }
    public void UpdateUICounter() {
        int maxSpot = 0;
        int openedSpot = 0;
        for (int i = 0; i < spotGroupManagers.Length; i++)
        {
            maxSpot += spotGroupManagers[i].HotSpotMaxCounter;
            openedSpot += spotGroupManagers[i].HotSpotOpenedCounter;
        }
        uIVounterHandler.TextCounterStr = $"{openedSpot} / {maxSpot}";
    }

    void StartQuiz() {
        for (int i = 0; i < spotGroupManagers.Length; i++)
        {
            spotGroupManagers[i].PrepaireQuiz();
        }
    }
}
