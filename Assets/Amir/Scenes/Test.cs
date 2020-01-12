using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Animator anim;
    public void PlayAnim() {
        anim.enabled = true;
    }
}
