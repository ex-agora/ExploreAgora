using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PSSUpperButtonsHandler : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Sprite activeBtnSprite;
    [SerializeField] int firstCommandIndex;
    [SerializeField] int secondCommandIndex;
    bool isFirstClick = true;
    PSSUpperButtonManager manager;
    public void Active ()
    {
        btn.interactable = false;
        btn.image.sprite = activeBtnSprite;
        manager.HandlerActive ();
    }
    public void ShowCommand ()
    {
        if ( isFirstClick )
        {
            //print (firstCommand);
            PhotosynthesisGameManager.Instance.ShowHint (firstCommandIndex);
            isFirstClick = false;
        }
        else
        {
            // print (secondCommand);
            PhotosynthesisGameManager.Instance.ShowHint (secondCommandIndex);
        }
    }
    public void SetManager (PSSUpperButtonManager _Manager)
    {
        manager = _Manager;
    }
}

