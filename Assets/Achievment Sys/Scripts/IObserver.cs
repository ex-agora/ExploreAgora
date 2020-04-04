using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver 
{
    void OnNotificationReceived(int x , ObservableHandler observable);
    void OnNotificationReceived(bool x , ObservableHandler observable);
}
