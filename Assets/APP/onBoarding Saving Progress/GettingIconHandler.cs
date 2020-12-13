using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GettingIconHandler : MonoBehaviour
{
    [SerializeField] UnityEvent GettingBehavior;

    public void DoGettingActions() {
        GettingBehavior.Invoke();
    }
}
