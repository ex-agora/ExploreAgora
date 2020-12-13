using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS17UpperBarEvents : MonoBehaviour
{
public void ActiveTrade ()
    {
        SS17GameManager.Instance.Trade.Active ();
    }
    public void ActiveColonization ()
    {
        SS17GameManager.Instance.Colonization.Active ();
    }
    public void ActiveDiplomatic ()
    {
        SS17GameManager.Instance.Diplomatic.Active ();
    }
}
