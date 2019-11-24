using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothObjHandler : MonoBehaviour
{
    [SerializeField] GameObject[] moths;
    public void WhiteMoothHit()
    {
        MothGameManager.Instance.WhiteMoothHit();
    }
    public void BlackMoothHit()
    {
        MothGameManager.Instance.BlackMoothHit();
    }
    public void ShowMoth()
    {
        for (int i = 0; i < moths.Length; i++)
        {
            moths[i].SetActive(true);
        }
    }
    private void Start()
    {
        HideMoth();
    }
    public void HideMoth()
    {
        for (int i = 0; i < moths.Length; i++)
        {
            moths[i].SetActive(false);
        }
    }
}
