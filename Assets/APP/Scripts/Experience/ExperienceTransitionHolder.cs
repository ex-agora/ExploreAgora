using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceTransitionHolder : MonoBehaviour
{
    ExperienceContainerHolder nextExperienceContainerHolder;
    string bundleID;
    static ExperienceTransitionHolder instance;
    public ExperienceContainerHolder NextExperienceContainerHolder { get => nextExperienceContainerHolder; set => nextExperienceContainerHolder = value; }
    public string BundleID { get => bundleID; set => bundleID = value; }
    public static ExperienceTransitionHolder Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
