using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SS17UpperButtonHandler : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Sprite activeBtnSprite;
    //[SerializeField] int firstCommandIndex;
    //[SerializeField] int secondCommandIndex;
    //bool isFirstClick = true;
    SS17UpperBarManager manager;
    public void Active ()
    {
        btn.interactable = false;
        btn.image.sprite = activeBtnSprite;
        //AudioManager.Instance?.Play("miniNotification", "Activity");
        manager.HandlerActive ();
    }
    //public void ShowCommand ()
    //{
    //    if ( isFirstClick )
    //    {
    //        //print (firstCommand);
    //        PhotosynthesisGameManager.Instance.ShowHint (firstCommandIndex);
    //        isFirstClick = false;
    //    }
    //    else
    //    {
    //        // print (secondCommand);
    //        PhotosynthesisGameManager.Instance.ShowHint (secondCommandIndex);
    //    }
    //}
    public void SetManager (SS17UpperBarManager _Manager)
    {
        manager = _Manager;
    }
}

