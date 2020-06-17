using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToWorldEventPlace : MonoBehaviour
{
    [SerializeField] string targetString;

    public string TargetString { get => targetString; set => targetString = value; }
}
