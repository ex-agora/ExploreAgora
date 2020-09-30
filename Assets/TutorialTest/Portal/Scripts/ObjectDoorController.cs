using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDoorController : MonoBehaviour
{
    private static readonly int Close = Animator.StringToHash("OpenClose");

    private void OnTriggerEnter(Collider other)
    {
        OpenClose();
    }

        public void OpenClose()
    {
        Animator anim = this.GetComponentInChildren<Animator>();
        anim.SetTrigger(Close);
    }

}
