using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterUIHandler : MonoBehaviour
{
    [SerializeField] Text textCounter;
    [SerializeField] Animator anim;

    public string TextCounterStr {  set => textCounter.text = value; }

    public void ShowCounter() => anim.SetBool("isOpen", true);
    public void HideCounter() => anim.SetBool("isOpen", false);
}
