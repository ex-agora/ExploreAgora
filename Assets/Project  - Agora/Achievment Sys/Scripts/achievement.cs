using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Achievement", menuName = "SO/Variable/Achievements", order = 0)]
public class achievement : ScriptableObject
{

    [SerializeField]  Sprite beforeAchievement;
   [SerializeField]  Sprite afterAchievement;
   [SerializeField] string title;
    [TextArea] [SerializeField] string description;
     bool state;
    string id= uniqueId();

    public string Id { get => id; }
    public Sprite BeforeAchievement { get => beforeAchievement; }
    public Sprite AfterAchievement { get => afterAchievement; }
    public string Title { get => title; }
    public string Description { get => description; }
    public bool State { get => state; set => state =value; }



    // generate A unique ID For Each Achievment to be diffrentiated within achievment mananger
    static string uniqueId()
    {
        Guid guid = Guid.NewGuid();
        string str = guid.ToString();
        return str;
    }
}