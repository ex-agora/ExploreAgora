using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersHandler : MonoBehaviour
{

    public void OpenColliders()
    {
        for (int i = 0; i < GetComponentsInChildren<BoxCollider>().Length; i++)
            GetComponentsInChildren<BoxCollider>()[i].enabled = true;
      }
}
