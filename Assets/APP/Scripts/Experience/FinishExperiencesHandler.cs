using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishExperiencesHandler : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] LoadScenePrefabs scenePrefabs;
    static FinishExperiencesHandler instance;
    bool isPressed = false;

    public static FinishExperiencesHandler Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void FinshExperience(int score) {
        if (scenePrefabs.GetExperience().experienceRate == 0) {
            AppManager.Instance.IsThereRate = true;
            AppManager.Instance.ExperienceCode = scenePrefabs.GetExperience().experienceCode;
        }
        ExperiencePlayData s = new ExperiencePlayData();
        s.status = 2;
        s.experienceCode = scenePrefabs.GetExperience().experienceCode;
        s.score = score;
        NetworkManager.Instance.UpdateExperienceStatus(s, OntUpdateExperienceSuccess, OntUpdateExperienceFailed);
    }
    public void GotoHome() { SceneLoader.Instance.LoadExperience(sceneName); }
    private void OntUpdateExperienceSuccess(NetworkParameters obj)
    {
        isPressed = false;
        SceneLoader.Instance.LoadExperience(sceneName);
    }
    private void OntUpdateExperienceFailed(NetworkParameters obj)
    {
        isPressed = false;
        SceneLoader.Instance.LoadExperience(sceneName);

    }
}
