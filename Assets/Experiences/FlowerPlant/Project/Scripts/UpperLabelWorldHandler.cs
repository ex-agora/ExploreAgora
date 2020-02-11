using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpperLabelWorldHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text tMP;
    [SerializeField] Image icon;
    string info;

    public string Info { get => info; set { info = value; UpdateText(); } }
    public void Show() => anim?.SetBool("isOpen", true);
    public void Hide() => anim?.SetBool("isOpen", false);
    void UpdateText() {
        icon.gameObject.SetActive(false);
        tMP.enableAutoSizing = true;
        tMP.text = info;
    }
}
