using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothObjHandler : MonoBehaviour
{
    public void WhiteMoothHit()
    {
        MothGameManager.Instance.WhiteMoothHit();
    }
    public void BlackMoothHit()
    {
        MothGameManager.Instance.BlackMoothHit();
    }
}
