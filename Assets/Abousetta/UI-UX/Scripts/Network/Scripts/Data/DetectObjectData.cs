public class DetectObjectData { 
    public string detectionObjectName;
    public byte [] bytes;
    public ObjectsToDetect objectsToDetect;
}
[System.Serializable]
public class ObjectsToDetect {
    public DetectObjectInfo[] objects;
}
[System.Serializable]
public class DetectObjectInfo {
    public string name;
    public string score;
}
