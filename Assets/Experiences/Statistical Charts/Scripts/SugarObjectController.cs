using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarObjectController : MonoBehaviour
{
    [SerializeField] private List<GameObject> sugars = new List<GameObject>();
    [SerializeField] private float updateRate = 0.6f;

    private int index;


    public void ShowingSugar()
    {
        if (!IsInvoking(nameof(ShowingSugarUpdate)))
        {
            index = 0;
            InvokeRepeating(nameof(ShowingSugarUpdate), 0, updateRate);
        }
    }

    private void ShowingSugarUpdate()
    {
        if (index < sugars.Count)
        {
            sugars[index].SetActive(true);
            AudioManager.Instance?.Play("miniNotification", "Activity");
            index++;
        }
        else
        {
            if (IsInvoking(nameof(ShowingSugarUpdate)))
                CancelInvoke(nameof(ShowingSugarUpdate));
        }
    }

}
