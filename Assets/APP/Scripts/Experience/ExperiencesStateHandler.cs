using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencesStateHandler : MonoBehaviour
{
    [SerializeField] StringExperienceContainerHolderDictionary experiences;
    public void HandleExperiencesStates()
    {
        NetworkManager.Instance.GetExperienceStatus(OnGetExperiencesSuccess, OnGetExperiencesFailed);
    }
    private void OnGetExperiencesSuccess(NetworkParameters obj)
    {
        string key;
        ExperienceResponse getExperienceResponse = (ExperienceResponse)obj.responseData;
        for (int i = 0; i < getExperienceResponse.experience.Length; i++)
        {
            key = getExperienceResponse.experience[i].experienceCode;
            experiences[key].experienceScore = getExperienceResponse.experience[i].maxScore;
            experiences[key].playedCounter = (uint)getExperienceResponse.experience[i].playedTimesCounter;
            experiences[key].finishedCounter = (uint)getExperienceResponse.experience[i].finishedTimesCounter;
        }
    }
    private void OnGetExperiencesFailed(NetworkParameters obj)
    {

        
        print(obj.err.message);
    }
}
