using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCube : MonoBehaviour
{

    #region Fields
    Vector3 newScale;
    float scalar;
    #endregion Fields

    #region Methods
    void Update()
    {
        scalar = Zoom.instance.dis / 10000;
        if (Mathf.Abs(Zoom.instance.ang) > 30 && Mathf.Abs(Zoom.instance.ang) < 60)
        {
            newScale = Vector3.one * scalar;
        }
        else if (Mathf.Abs(Zoom.instance.ang) < 30)
        {
            newScale = new Vector3(1, 0, 1) * scalar;
        }
        else if (Mathf.Abs(Zoom.instance.ang) > 60)
        {
            newScale = new Vector3(0, 1, 1) * scalar;
        }
        else if (Mathf.Abs(Zoom.instance.ang) == 0)
        {
            newScale = Vector3.zero;
        }
        else
        {
            newScale = Vector3.zero;
        }
        if (!Zoom.instance.minus)
        {
            transform.localScale += newScale;
        }else {
            transform.localScale -= newScale;
        }
    }
    #endregion Methods
}
