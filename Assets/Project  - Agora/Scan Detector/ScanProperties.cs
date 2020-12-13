using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "ScanProperties", menuName = "SO/Variable/Scan Properties", order = 0)]

public class ScanProperties : ScriptableObject
{
    /* this string which contained in scriptableobject responsible of holding the name of object that 
     * need to be scanned currently */
    //#region Fields
    public string detectionObjectName;
    public Sprite detectionObjectSp;
    public Sprite outlineSp;
    public List<DetectObjectInfo> objectInfos;
}
