using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpperLabelWorldHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text tMP;
    string info;

    public string Info { get => info; set => info = value; }
    public void Show() => anim?.SetBool("isOpen", true);
    public void Hide() => anim?.SetBool("isOpen", false);
}
