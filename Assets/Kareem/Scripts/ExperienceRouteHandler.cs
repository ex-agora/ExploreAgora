using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class ExperienceRouteHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] SOTest @sceneNavManager;
    [SerializeField] string SceneName;
    static ExperienceRouteHandler instance;

    public static ExperienceRouteHandler Instance { get => instance; set => instance = value; }
    #endregion Fields
    #region Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void Transit (ExperienceContainerHolder experienceContainerHolder)
    {
        //Debug.Log ("SceneName " + SceneName);
        //Debug.Log ("experienceContainerHolder " + experienceContainerHolder);
        //Debug.Log ("experienceContainerHolder experiencePrefab" + experienceContainerHolder.experiencePrefab);
        //Debug.Log ("experienceContainerHolder experienceName" + experienceContainerHolder.experienceName);
        //Debug.Log ("experienceContainerHolder experienceCode" + experienceContainerHolder.experienceCode);
        //Debug.Log ("experienceContainerHolder scannedObject" + experienceContainerHolder.scannedObject);
        //Debug.Log ("experienceContainerHolder categories.Length" + experienceContainerHolder.categories.Length);
        //Debug.Log ("experienceContainerHolder topics.Length" + experienceContainerHolder.topics.Length);
        //Debug.Log ("experienceContainerHolder isIndoor" + experienceContainerHolder.isIndoor);
        //Debug.Log ("experienceContainerHolder tags.Length" + experienceContainerHolder.tags.Length);
        //Debug.Log ("experienceContainerHolder minimumAgeGroup" + experienceContainerHolder.minimumAgeGroup);
        //Debug.Log ("experienceContainerHolder maximumAgeGroup" + experienceContainerHolder.maximumAgeGroup);
        //Debug.Log ("experienceContainerHolder subject" + experienceContainerHolder.subject);
        //Debug.Log ("experienceContainerHolder requiredArea" + experienceContainerHolder.requiredArea);
        //Debug.Log ("experienceContainerHolder token" + experienceContainerHolder.token);
        Debug.Log ("@scenesPrefabsIntializers " + @sceneNavManager);
        if ( experienceContainerHolder == null)
        {
            Debug.LogWarning ("Transit function needs experienceContainerHolder parameter");
            return;
        }
        @sceneNavManager.nextExperienceContainerHolder = experienceContainerHolder;
        Debug.Log ("@scenesPrefabsIntializers.nextExperienceContainerHolder" + @sceneNavManager.nextExperienceContainerHolder);
        SceneLoader.Instance.LoadExperience (SceneName);
    }
    #endregion Methods
}