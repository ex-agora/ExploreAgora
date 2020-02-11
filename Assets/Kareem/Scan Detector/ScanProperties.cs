using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Required Object Name", menuName = "SO/Variable/Required Object Name", order = 0)]

public class ScanProperties : ScriptableObject
{
    /* this string which contained in scriptableobject responsible of holding the name of object that 
     * need to be scanned currently */
    //#region Fields
    public string detectionObjectName;
    [SerializeField] bool shouldContinueToExperience;

    public bool ShouldContinueToExperience{ get => shouldContinueToExperience; set => shouldContinueToExperience = value; }
}
