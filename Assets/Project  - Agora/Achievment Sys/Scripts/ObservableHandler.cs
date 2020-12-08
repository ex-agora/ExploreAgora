using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObservableHandler : MonoBehaviour
{

    // define type of achievment in this case it depends on A counter
    [HideInInspector] [SerializeField] bool isCount;
    // define type of achievment in this case it depends on An Event
    [HideInInspector] [SerializeField] bool isEvent;

    [SerializeField] ObservableType observableType;
    [ConditionalHide(nameof(isEvent), true)] [SerializeField] System.Func<bool> eventAction;
    [ConditionalHide(nameof(isCount), true)] [SerializeField] int count;
    List<IObserver> observers = new List<IObserver>();
    public int Count { get => count; set => count = value; }
    public Func<bool> EventAction { get => eventAction; set => eventAction = value; }
    private void Start()
    {
        isCount = true;
        isEvent = false;
    }

    // RegisterObservers :)
    public void RegisterObservers(IObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    // UnRegisterObservers :)
    public void UnRegisterObservers(IObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    // loop over all observers and notify them if their condition value changed 
    public void NotifyObservers()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            switch (observableType)
            {
                case ObservableType.Depend_On_Counter:
                    observers[i].OnNotificationReceived(Count, this);
                    break;
                case ObservableType.Depend_On_Event:
                    bool up = EventAction == null ? false : EventAction.Invoke();
                    observers[i].OnNotificationReceived(up, this);
                    break;
            }

        }
    }
    private void OnValidate()
    {
        // Editor Actions
        // if observableType was counter then counter field only will appear else event fields will
        switch (observableType)
        {
            case ObservableType.Depend_On_Counter:
                isCount = true;
                isEvent = false;
                break;
            case ObservableType.Depend_On_Event:
                isCount = false;
                isEvent = true;
                break;
        }
    }
}
