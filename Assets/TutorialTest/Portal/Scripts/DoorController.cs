using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int f;

    private static readonly int Close = Animator.StringToHash("OpenClose");
    // private Button DoorToggle;

   private void Awake()
    {
        f = 5;
        //DoorToggle.onClick.AddListener(OpenClose);

    }

    public void OpenClose()
    {
        var anim = this.GetComponentInChildren<Animator>();
        anim.SetTrigger(Close);
    }

}
