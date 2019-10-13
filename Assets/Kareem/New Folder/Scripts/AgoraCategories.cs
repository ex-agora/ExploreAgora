using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "AgoraCategories", menuName = "SO/Experience/AgoraCategories", order = 0)]
public class AgoraCategories : ScriptableObject
{
    [HideInInspector]
    public static List<string> categories = new List<string>();
    public string newCategory;


    public void AddNewCategory()
    {
        if (newCategory != "")
            if (!categories.Contains(newCategory))
            {
                categories.Add(newCategory);
            }
            else
            {
                Debug.LogError($"{newCategory} This Category already exists!");
            }
    }
    public void DeleteCategory()
    {
        if (newCategory != "")
            if (categories.Contains(newCategory))
            {
                categories.Remove(newCategory);
            }
            else
            {
                Debug.LogError($"{newCategory} does not exist!");
            }
    }
}