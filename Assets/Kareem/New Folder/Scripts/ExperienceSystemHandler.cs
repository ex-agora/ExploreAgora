using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExperienceSystemHandler : MonoBehaviour
{
    #region Fields
    [Space(10)]
    [Header("Add category")]
    public string category;

    public int currentScore;

    public EDetectionType detectionType;

    [HideInInspector]
    public List<string> experienceCategory;

    public string experienceName;

    public string experienceSubTitle;

    public List<string> experienceTag;

    [Tooltip("example ya amir beeeeh")]
    public EGroupAge groupAge;

    public Icon icon;

    public string iD;

    public int maxScore;

    public int playedCount;

    public int repeatedPlayedTime;

    // public AgoraCategoriesEditor categories;
    public Tag tags;
    #endregion Fields

    //Validation
    /*----------------------------- */
    /*----------------------------- */
    #region Methods
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

    // the number of playing the whole experience
    private void OnValidate()
    {

    }
    #endregion Methods
}
