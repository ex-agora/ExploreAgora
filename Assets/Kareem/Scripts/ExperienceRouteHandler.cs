using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class ExperienceRouteHandler : MonoBehaviour
{
    #region Fields
    //[SerializeField] SOTest @sceneNavManager;
    [SerializeField] string SceneName;
    [SerializeField] AchievementHolder achievement;
    SceneLoader sceneLoader;
    static ExperienceRouteHandler instance;
    bool isPressed = false;
    public static ExperienceRouteHandler Instance { get => instance; set => instance = value; }
    #endregion Fields
    #region Methods
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void Transit(ExperienceContainerHolder experienceContainerHolder,BundleHandler bundle, SceneLoader _sceneLoader =null)
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
       // Debug.Log("@scenesPrefabsIntializers " + @sceneNavManager);
        if (experienceContainerHolder == null)
        {
            Debug.LogWarning("Transit function needs experienceContainerHolder parameter");
            return;
        }
        if (isPressed)
            return;
        isPressed = true;
        //Debug.LogWarning(@sceneNavManager.nextExperienceContainerHolder + " NNNNNNNNNNNNNNNNNNN ");
        ExperienceTransitionHolder.Instance.NextExperienceContainerHolder = experienceContainerHolder;
        //Debug.LogWarning(@sceneNavManager.nextExperienceContainerHolder + " qqqqqqqqqqqqqqqqqq ");
        ExperienceTransitionHolder.Instance.BundleID = bundle?.BundleID;
        ExperiencePlayData s = new ExperiencePlayData(); s.status = 1;
        s.experienceCode = experienceContainerHolder.experienceCode;
        s.score = 0;
        if (experienceContainerHolder != null)
        {
            if (experienceContainerHolder.playedCounter > 1)
            {
                achievement.UpdateCurrent();
                Sprite badge = achievement.GetBadge();
                if (badge != null)
                {
                    AchievementManager.Instance.AddBadge(badge);
                }
            }
        }
        sceneLoader = _sceneLoader;
        if (TestingManager.Instance.IsTestStart)
        {
            OntUpdateExperienceSuccess(null);
        }
        else
        {
            NetworkManager.Instance.UpdateExperienceStatus(s, OntUpdateExperienceSuccess, OntUpdateExperienceFailed);
            // Debug.Log ("@scenesPrefabsIntializers.nextExperienceContainerHolder" + @sceneNavManager.nextExperienceContainerHolder);
        }
    }

    public void TransitTest(ExperienceContainerHolder experienceContainerHolder) {

        ExperienceTransitionHolder.Instance.NextExperienceContainerHolder = experienceContainerHolder;

        SceneLoader.Instance.LoadExperience(SceneName);
    }
    private void OntUpdateExperienceSuccess(NetworkParameters obj)
    {
        isPressed = false;
        if (sceneLoader is null)
            SceneLoader.Instance.LoadExperience(SceneName);
        else
            sceneLoader.LoadExperience(SceneName);
    }
    private void OntUpdateExperienceFailed(NetworkParameters obj)
    {
        isPressed = false;
        if (UXFlowManager.Instance.IsThereNetworkError(obj.err.errorTypes))
            return;
        //SceneLoader.Instance.Loading();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    #endregion Methods
}