using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoadingScienceObjSetter : MonoBehaviour
{
    [SerializeField] FadeInOut bookCoverObj;
    [SerializeField] FadeInOut bookPaperObj;
    [SerializeField] FadeInOut bookLockCoverObj;
    [SerializeField] FadeInOut bookP1Obj;
    [SerializeField] FadeInOut bookP2Obj;
    [SerializeField] FadeInOut bookP3Obj;
    [SerializeField] FadeInOut bookP4Obj;
    [SerializeField] FadeInOut bookP5Obj;
    [SerializeField] GameObject coalObj;

    public void ObjectContainSetter()
    {
        FadeOutBook();
        Invoke(nameof(ActeiveCoalObj), 2.5f);
    }
    void ActeiveCoalObj() {
        coalObj.SetActive(true);
    }
    void FadeOutBook() {
        bookCoverObj.fadeInOut(false);
        bookPaperObj.fadeInOut(false);
        bookLockCoverObj.fadeInOut(false);
        bookP1Obj.fadeInOut(false);
        bookP2Obj.fadeInOut(false);
        bookP3Obj.fadeInOut(false);
        bookP4Obj.fadeInOut(false);
        bookP5Obj.fadeInOut(false);
    }
}
