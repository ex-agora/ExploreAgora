using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialFlowManager : MonoBehaviour
{
    private TutorialPhase phase;
    [SerializeField] private Animator eyeBlick;
    [SerializeField] private ToolBarHandler startInstruction;
    [SerializeField] private ToolBarHandler endInstruction;
    [SerializeField] private InstructionHandler instructionHandler; 
    public static TutorialFlowManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
       
    }

    private void Start()
    {
        AudioManager.Instance.Play("bg", "Background");
        EnablePhase();
        NextState();
        
    }

    private void SetNextPhase()
    {
        phase++;
    }
    public void AppleFeedback()
    {
        AudioManager.Instance.Play("revealObject", "Activity");
        instructionHandler.CorrectFollowFeedback();
    }


    private void EnablePhase()
    {
        var str = string.Empty;
        switch (phase)
        {
            case TutorialPhase.None:
                interactions.Instance.CloseDetection(); break;
            case TutorialPhase.Init:
                str = "Tap On Ready Button";
                startInstruction.OpenToolBar();
                instructionHandler.OpenInstruction();
                break;
            case TutorialPhase.EyeBlink:
                str = "Scan The Ground";
                startInstruction.CloseToolBar();
                eyeBlick.enabled = true;
                interactions.Instance.OpenDetection();
                break;
            case TutorialPhase.Placing:
                str = "Move To Green Light";
                TutorialPlacingHandler.Instance.HandlePoint();
                break;
            case TutorialPhase.MoveTo1:
                TutorialPlacingHandler.Instance.HandlePoint();
                str = "Move To Green Light";
                break;
            case TutorialPhase.MoveTo2:
                TutorialPlacingHandler.Instance.HandlePoint();
                str = "Move To Green Light";
                break;
            case TutorialPhase.MoveTo3:
                TutorialPlacingHandler.Instance.HandlePoint();
                str = "";
                break;
            case TutorialPhase.Dragging:
                str = "Tap and Move The Mango to Correct Place";
                TutorialPlacingHandler.Instance.EnableDrag();
                break;
            case TutorialPhase.Portal:
                str = "Move and Enter The Portal";
                TutorialPlacingHandler.Instance.DragDone();
                TutorialPlacingHandler.Instance.StartPortal();
                break;
            case TutorialPhase.End:
                str = "";
                endInstruction.OpenToolBar();
                break;
            default:
                phase = TutorialPhase.None;
                break;
        }
        
        SetNextInstructionTxt(str);
    }

    private void SetNextInstructionTxt(string str)
    {
        instructionHandler.SetInstrucation(str);
    }

    public void NextState()
    {
        SetNextPhase();
        EnablePhase();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
