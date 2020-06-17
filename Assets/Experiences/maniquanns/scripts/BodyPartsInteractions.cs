using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsInteractions : MonoBehaviour
{
    [SerializeField]EBodyParts bodyParts;
    public void changePart()
    {

        BodyPartsHandler.Instance.ChangeBodyParts(bodyParts);
    }
}
