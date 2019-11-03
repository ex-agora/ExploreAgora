using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineStateHandler : MonoBehaviour
{
    [SerializeField] List<OutlineHandler> handlers;
    [SerializeField] Color outlineColor;
    private void Start()
    {
        for (int i = 0; i < handlers.Count; i++)
        {
            handlers[i].SetColor(outlineColor);
        }
    }

    public void ShowOutline() {
        for (int i = 0; i < handlers.Count; i++)
        {
            handlers[i].ShowOutline();
        }
        Invoke(nameof(HideOutline), 2f);
    }
    public void HideOutline()
    {
        for (int i = 0; i < handlers.Count; i++)
        {
            handlers[i].HideOutline();
        }
    }



}
