using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoadingScienceObjSetter : MonoBehaviour
{
    [SerializeField] GameObject bookObj;
    [SerializeField] GameObject coalObj;

    public void ObjectContainSetter()
    {
        bookObj.SetActive(false);
        coalObj.SetActive(true);
    }
}
