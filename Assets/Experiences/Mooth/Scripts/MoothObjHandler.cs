using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoothObjHandler : MonoBehaviour
{
    public void WhiteMoothHit()
    {
        MoothGameManager.Instance.WhiteMoothHit();
    }
    public void BlackMoothHit()
    {
        MoothGameManager.Instance.BlackMoothHit();
    }
}
