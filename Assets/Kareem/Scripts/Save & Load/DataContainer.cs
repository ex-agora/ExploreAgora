using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class DataSerializer
{

    public string cat;
    public int someIntValue;
    public string name;
    public DataSerializer (DataContainer dc)
    {
        cat = dc.category;
        someIntValue = dc.someint;
        name = dc.categoryyyy;
    }
}

public class DataContainer : MonoBehaviour
{
    public string category;
    public string categoryyyy;
    public int someint;

    public void save ()
    {
        SaveLoadHandler.Save ("Activity", this);
    }
    public void saveAppend ()
    {
        SaveLoadHandler.Save ("Activity", this, true);
    }
    public void load ()
    {
        SaveLoadHandler.Load ("Activity");
    }

}