using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGroupHandler : MonoBehaviour
{
    [SerializeField] Animator anim;
    bool isOpen;
    private void Start()
    {
        isOpen = false;
    }

    private void UpdateAnim() => anim.SetBool("IsOpen", isOpen);
    public void ToggelOpen() {
        isOpen = !isOpen;
        UpdateAnim();
    }
}
