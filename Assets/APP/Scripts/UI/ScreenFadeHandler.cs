using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFadeHandler : MonoBehaviour
{
    [SerializeField] Animator fadeHandler;
    [SerializeField] float duration;
    void Awake()
    {
        Invoke(nameof(FadeOut), duration);
    }

    void FadeOut() => fadeHandler.SetTrigger("FadeOut");
}
