using UnityEngine;
using System.Collections;

public class AddCategory : MonoBehaviour
{
    #region Fields
    public GameObject obj;
    public Vector3 spawnPoint;
    #endregion Fields


    #region Methods
    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, Quaternion.identity);
    }
    #endregion Methods
}