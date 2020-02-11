using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    #region Fields
    [SerializeField] Animator anim;
    #endregion Fields

    #region Methods
    public void PlayAnim() {
        anim.enabled = true;
    }
    #endregion Methods
}
