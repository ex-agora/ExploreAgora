using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    static AchievementManager instance;
    ulong score = 0;
    [HideInInspector][SerializeField]List<Sprite> badges;
    Sprite scannedObjectSp = null;
    string scannedName = string.Empty;
    [SerializeField] Sprite levelUpSp;
    bool isLevelup;
    public static AchievementManager Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    public AchiemvenetResult CalculateAchievements() {
        AchiemvenetResult result = new AchiemvenetResult();
        result.isScoreOnly = true;
        if (scannedName != string.Empty) {
            result.isScoreOnly = false;
            result.isObjectScanned = true;
            result.scannedName = scannedName;
            result.scannedObjectSp = scannedObjectSp;
        }
        if (badges.Count>0)
        {
            result.isScoreOnly = false;
            result.badges = new List<Sprite>(badges);
        }
        if (isLevelup) {
            result.isScoreOnly = false;
            result.isLevelUp = true;
            result.levelUpSp = levelUpSp;
        }


        if (result.isScoreOnly && score == 0)
            return null;
        else {

            result.score = score;
            score = 0;
            badges.Clear();
            scannedName = string.Empty;
            scannedObjectSp = null;
            isLevelup = false;
            return result;
        }
    }
    public void AddScore(uint _score) => score += _score;
    public void AddBadge(Sprite _badge) => badges.Add(_badge);
    public void AddScannedObject(Sprite _scannedSp, string _scannedName) { scannedName = _scannedName; scannedObjectSp = _scannedSp; }
    public void AddLevel() => isLevelup = true;
}
public class AchiemvenetResult {
   public ulong score;
   public List<Sprite> badges;
   public Sprite scannedObjectSp = null;
   public string scannedName = string.Empty;
   public Sprite levelUpSp;
   public bool isScoreOnly;
   public bool isObjectScanned;
   public bool isLevelUp;
}