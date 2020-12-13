[System.Serializable]
public class GeneralResponse<T> where T : ResponseData{
    public bool success;
    public T data;
}
