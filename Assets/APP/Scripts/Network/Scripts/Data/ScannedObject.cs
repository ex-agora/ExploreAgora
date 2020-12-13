[System.Serializable]
public class ScannedObject 
{
    public string name;
    public int counter;
    public ScannedObject(string _name = "",int _counter =0) {
        name = _name;
        counter = _counter;
    }
}
