using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DataContainer : MonoBehaviour
{
    #region Fields
    public string category;
    public string categoryyyy;
    public int someint;
    #endregion Fields

    #region Methods
    public void load()
    {
        SaveLoadHandler.Load("Activity");
    }

    public void save()
    {
        SaveLoadHandler.Save("Activity", this);
    }
    public void saveAppend()
    {
        SaveLoadHandler.Save("Activity", this, true);
    }
    #endregion Methods
}

[Serializable]
public class DataSerializer
{

    #region Fields
    public string cat;
    public string name;
    public int someIntValue;
    #endregion Fields

    #region Constructors
    public DataSerializer (DataContainer dc)
    {
        cat = dc.category;
        someIntValue = dc.someint;
        name = dc.categoryyyy;
    }
    #endregion Constructors
}
