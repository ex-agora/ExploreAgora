using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] private Text insTxt;
    public void OpenInstruction() => anim.enabled = true;
    public void CloseInstruction() => anim.enabled = false;
    
    public void CorrectFollowFeedback() => anim.SetTrigger("CorrectFollow");
    public void SetInstrucation(string str) => insTxt.text = str;

    public void FollowCorrcelty() => TutorialFlowManager.Instance.NextState();
}