[System.Serializable]
public class ExperienceStatusResponse
{
    public int playedTiemesCounter;
    public int finishedTimesCounter;
    public int totalPlayedDuration; //in millisecond
    public int maxTimesCounter;  //in milliseconds
    public int maxScore;  
    public string lastPlayedAt;  
    public string experienceName;  
}
