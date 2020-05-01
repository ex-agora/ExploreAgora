using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameplayManager : MonoBehaviour
{
    [SerializeField] ProfileNetworkHandler profileNetwork;
    [SerializeField] InventoryObjectHolder inventory;
    [SerializeField] ExperiencesStateHandler experiences;
    static ResetGameplayManager instance;

    public static ResetGameplayManager Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ResetAccount() {
        experiences.ResetExperiences();
        inventory.ResetInventory();
        profileNetwork.ResetProfile();
    }

    public void GetExperiences() {
        experiences.HandleExperiencesStates();
    }
}
