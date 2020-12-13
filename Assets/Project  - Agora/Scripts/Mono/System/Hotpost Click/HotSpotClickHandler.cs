using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotClickHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] GameEvent afterClickHotspot , gameEvent;
    bool firstTime = true;
    [SerializeField] GameEventListener lis;
    #endregion Fields

    #region Methods
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
    #endregion Methods 
}
