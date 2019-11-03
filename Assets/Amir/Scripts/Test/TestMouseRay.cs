using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseRay : MonoBehaviour
{
    [SerializeField] Camera cam;
    Ray ray;
    RaycastHit hit;
    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
