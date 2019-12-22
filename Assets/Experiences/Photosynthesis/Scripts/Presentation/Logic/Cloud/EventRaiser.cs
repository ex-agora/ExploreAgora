using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRaiser : MonoBehaviour
{
    [SerializeField] GameEvent @event;
    public void Fire() {
        @event.Raise();
    }

}
