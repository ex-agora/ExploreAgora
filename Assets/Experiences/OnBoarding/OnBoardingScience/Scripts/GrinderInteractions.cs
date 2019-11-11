using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderInteractions : MonoBehaviour
{
    [SerializeField] GameEvent afterGrinderComplete;
    [SerializeField] GameObject coalPowder;
    [SerializeField] Animator coalGrinderAnim;
    float updateRate = 1;
    float duration = 5;

    void PlaceCoalPowder()
    {
        coalPowder.SetActive(true);
    }

    void CustomUpdate()
    {
        duration -= updateRate;
        if (duration <= 0)
        {
            CancelInvoke(nameof(CustomUpdate));
            afterGrinderComplete?.Raise();
            coalGrinderAnim.SetTrigger("Fire");
        }
    }
    public void StartGrinder()
    {
        duration = 4.3f;
        coalGrinderAnim.SetTrigger("Fire");
        AudioManager.Instance.Play("grinder", "Activity");
        InvokeRepeating(nameof(CustomUpdate), 0, updateRate);
    }
   public void ShowPowder()
    {
        Invoke(nameof(PlaceCoalPowder), 0.65f);
    }

}