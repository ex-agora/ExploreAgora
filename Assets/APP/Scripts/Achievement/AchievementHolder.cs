using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AchievementHolder", menuName = "SO/Variable/Achievement Holder", order = 0)]
public class AchievementHolder : ScriptableObject
{
    [SerializeField] public Sprite blueSprite;
    [SerializeField] public Sprite purpleSprite;
    [SerializeField] public Sprite goldSprite;
                     
    [SerializeField] public int blueRange;
    [SerializeField] public int purpleRange;
    [SerializeField] public int goldRange;
                     
    [SerializeField] public int current;

    public void SetCurrent(int _current) => current = _current;
    public void UpdateCurrent() => current++;
    public Sprite GetBadge() {
        if (current == blueRange)
            return blueSprite;
        else if (current == purpleRange)
            return purpleSprite;
        else if (current == goldRange)
            return goldSprite;
        else
            return null;
    }

}
