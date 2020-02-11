using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDragEventRaiser : MonoBehaviour
{
    [SerializeField] Lean.Touch.LeanSelectable selectable;
    [SerializeField] UnityEvent eventSystem;
    private void Update()
    {
        if (transform.hasChanged /*&& selectable.IsSelected)*/ ){
            eventSystem?.Invoke();
        }
    }
}
