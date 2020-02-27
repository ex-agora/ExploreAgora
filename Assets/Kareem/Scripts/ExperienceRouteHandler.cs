using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class ExperienceRouteHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] ScenesPrefabsIntializers @scenesPrefabsIntializers;
    [SerializeField] string SceneName;
    #endregion Fields


    #region Methods
    public void Transit (ExperienceContainerHolder experienceContainerHolder)
    {
        if ( experienceContainerHolder == null )
        {
            Debug.LogWarning ("Transit function needs experienceContainerHolder parameter");
            return;
        }
        @scenesPrefabsIntializers.nextExperienceContainerHolder = experienceContainerHolder;
        SceneManager.LoadScene (SceneName);
    }
    #endregion Methods
}