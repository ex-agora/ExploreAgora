using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExperienceSystemHandler : MonoBehaviour
{
    // public AgoraCategoriesEditor categories;
    public Tag tags;
    public string experienceName;
    public string experienceSubTitle;
    public string iD;//Validation
    public Icon icon;

    /*----------------------------- */
    [HideInInspector]
    public List<string> experienceCategory;
    public List<string> experienceTag;
    /*----------------------------- */
    public EDetectionType detectionType;
    public int playedCount;
    public int repeatedPlayedTime; // the number of playing the whole experience
    [Tooltip("example ya amir beeeeh")]
    public EGroupAge groupAge;
    public int maxScore;
    public int currentScore;
    [Space(10)]
    [Header("Add category")]
    public string category;

    private void OnValidate()
    {

    }

    public void AddNewCategory()
    {
        if (category != "")
        {
            if (AgoraCategories.categories.Contains(category))
            {
                if (!experienceCategory.Contains(category))
                {
                    experienceCategory.Add(category);
                }
                else
                {
                    Debug.LogError($"{category} This Category already exists!");
                }
            }
            else
            {
                Debug.LogError($"AgoraCategories does not {category}!");
            }
        }
    }
}
