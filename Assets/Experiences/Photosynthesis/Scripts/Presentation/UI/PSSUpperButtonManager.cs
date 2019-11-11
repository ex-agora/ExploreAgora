using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSUpperButtonManager : MonoBehaviour
{
    [SerializeField] GameEvent btnHandlerActive;
    [SerializeField] List<PSSUpperButtonsHandler> upperButtonsHandlers;
    int handlerCounter;
    // Start is called before the first frame update
    void Start ()
    {
        handlerCounter = 0;
        for ( int i = 0 ; i < upperButtonsHandlers.Count ; i++ )
        {

            upperButtonsHandlers [i].SetManager (this);
        }
    }
    public void HandlerActive ()
    {
        handlerCounter++;
        if ( handlerCounter == upperButtonsHandlers.Count )
        {
            btnHandlerActive?.Raise ();
            if( btnHandlerActive == null )
            {
                Debug.LogWarning ("No game event in PSSHandler script");
            }
        }
    }
}
