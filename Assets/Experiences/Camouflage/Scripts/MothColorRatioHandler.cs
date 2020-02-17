using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothColorRatioHandler : MonoBehaviour
{
    [SerializeField] float mothCurrentRatio ;
    [SerializeField] bool alreadyPlayed;
    public float MothCurrentRatio { get => mothCurrentRatio; set => mothCurrentRatio = value; }
    public bool AlreadyPlayed { get => alreadyPlayed; set => alreadyPlayed = value; }
}
