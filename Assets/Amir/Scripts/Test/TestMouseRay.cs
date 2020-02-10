using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseRay : MonoBehaviour
{
    #region Fields
    [SerializeField] Camera cam;
    RaycastHit hit;
    Ray ray;
    #endregion Fields

    #region Methods
    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
    #endregion Methods
}
