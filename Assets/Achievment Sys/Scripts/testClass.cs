using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class testClass : MonoBehaviour
{
    [SerializeField]TMP_InputField tmp_InputField;
    public TestObservable t;
    [SerializeField] GameEvent ge =null;
    int x;
    int X
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
            t.Count = x;
            raiseEvent();
        }
    } 

     
    public void changeXvalue()
    {
        X = Int32.Parse(tmp_InputField.text) ;
    }

    void raiseEvent()
    {
        ge?.Raise();
    }
}
