using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFadeHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void Start()
    {
        animator.keepAnimatorControllerStateOnDisable = true;
    }
    public void FadeIn() { animator.SetBool("IsFadeIn", true); }
    public void FadeOut() { animator.SetBool("IsFadeIn", false); }
}
