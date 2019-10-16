using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotClickHandler : MonoBehaviour
{
    [SerializeField] GameEvent afterClickHotspot , gameEvent;
    [SerializeField] GameEventListener lis;
    bool firstTime = true;
    public void raiseHotSpotEvent()
    {
        if (!firstTime)
            return;
        else
        {
            afterClickHotspot.Raise();
            Destroy(lis,0.5f);
           // gameEvent.UnSubscribe(lis);
            firstTime = false;
        }

    } 
}
