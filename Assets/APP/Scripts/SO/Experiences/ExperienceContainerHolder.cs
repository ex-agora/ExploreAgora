using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
/*
 * this script handle experince data 
 * GO prefabEXper
 * string {ExpName , ScoreExp , experienceId}
 * uint playedCounter   etla3abet         kam mara   [be moagarad ma ad5ol el exp => counter++]
 * uint finishedCounter etla3abet (kamla) kam mara   [lazem el exp te5las(Show final summary) => counter++]
 * string [] tags
 * string [] categories
 * bool isHaveToken
 * ExperienceToken token {ExperienceToken struct holds token data}
 * minimumAgeGroup  
 * maximumAgeGroup
 * scannedObject 
 * string [] topics
 * bool isIndoor
 * float requiredArea is a reuired area for playing [ely fel find the flat surface (model size in AR)]
 */
[CreateAssetMenu (fileName = "Experience Data" , menuName = "SO/App/Experience/ExperienceData" , order = 0)]
public class ExperienceContainerHolder : ScriptableObject
{
    public  string experiencePrefab;
    public string experienceName;
    public string experienceCode;
    public string scannedObject;
    public string [] categories;
    public string [] topics;
    public bool isIndoor;
    public bool hasExtra;
    public string [] tags;
    public string [] standardNGSS;
    public string [] standardEGY;
    public uint minimumAgeGroup;
    public uint maximumAgeGroup;
    public string subject;
    [TextArea]
    public string description;
    public string palceOn;
    public ExperienceRequiredArea requiredArea;
    [Range (0 , 3)] public int experienceScore;
    public bool shouldNotRate;
    [Range (0 , 3)] public int experienceRate;
    public uint playedCounter;
    public uint finishedCounter;
    public bool hasToken;
    public ExperienceToken token;
    public bool isReadyToPlay;
    public bool isActive;
}
