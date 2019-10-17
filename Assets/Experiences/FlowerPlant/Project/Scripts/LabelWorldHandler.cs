using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelWorldHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TMP_Text labelText;
    bool isOpen;
    // Start is called before the first frame update
    private void OnEnable ()
    {
        OpenLabel ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OpenLabel ()
    {
        anim?.SetBool ("isOpen" , isOpen);

    }
    public void ShowLabel ()
    {
        isOpen = true;
        OpenLabel ();
    }
}
