using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent @event;
    [SerializeField] UnityEvent eventSystem;
    private void OnEnable()
    {
        @event.Subscribe(this);
    }
    private void OnDisable()
    {
        @event.UnSubscribe(this);
    }
    public void Fire() {
        eventSystem.Invoke();
    }
   

}
