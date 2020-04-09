using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private ExperienceContainerHolder experienceContainerHolder;
    [SerializeField] private Button experienceButton;
    [SerializeField] private Sprite played;
    [SerializeField] private Sprite disactiveSprite;
    [SerializeField] private Sprite readyToPlay;
    [SerializeField] private Image gem;
    [SerializeField] private List<Sprite> gemStates;
    [SerializeField] private List<ExperienceHandler> experienceDependencies;
    [SerializeField] private PopupExperienceHandler popup;
    [SerializeField] private ScanChecker scan;
    [SerializeField] private BundleHandler bundle;
    public void UpdateUIState()
    {
        gem.enabled = false;
        experienceButton.interactable = false;

        if (experienceContainerHolder.playedCounter > 0 && experienceContainerHolder.isReadyToPlay && experienceContainerHolder.isActive)
        {
            gem.enabled = true;
            gem.sprite = gemStates[experienceContainerHolder.experienceScore];
            experienceButton.image.sprite = played;
            experienceButton.interactable = true;
            // Open Popup with one button
            ActiveDependencies();
            UnlockDependencies();
        }
        else if (experienceContainerHolder.isReadyToPlay && experienceContainerHolder.isActive)
        {
            // Open Popup with one button
            experienceButton.image.sprite = readyToPlay;
            experienceButton.interactable = true;
        }
        else if (experienceContainerHolder.isActive)
        {
            // Open Popup with two button
            experienceButton.image.sprite = disactiveSprite;
            experienceButton.interactable = true;
        }
    }
    public void LockExperience() => experienceContainerHolder.isReadyToPlay = false;

    public void UnlockExperience()
    {
        experienceContainerHolder.isReadyToPlay = true; UpdateUIState();
    }

    public void ActiveExperience()
    {
        experienceContainerHolder.isActive = true; UpdateUIState();
    }

    public void UnlockDependencies()
    {
        for (int i = 0; i < experienceDependencies.Count; i++)
            experienceDependencies[i].UnlockExperience();
    }

    public void ActiveDependencies()
    {
        for (int i = 0; i < experienceDependencies.Count; i++)
            experienceDependencies[i].ActiveExperience();
    }

    public void OpenPopup() {
        if (!experienceContainerHolder.isActive)
            return;
        bool up = false;

        popup.TitleStr = experienceContainerHolder.experienceName;
        popup.DescriptionStr = experienceContainerHolder.description;
        popup._Experience = experienceContainerHolder;
        popup._Bundle = bundle;
        if (experienceContainerHolder.playedCounter > 0 && experienceContainerHolder.isReadyToPlay && experienceContainerHolder.isActive)
        {
            up = false;
        }
        else if (experienceContainerHolder.isReadyToPlay && experienceContainerHolder.isActive)
        {
            up = false;
        }
        else if (experienceContainerHolder.isActive)
        {
            popup._Scan = scan;
            up = true;
           
        }
        popup.OpenPopup(experienceContainerHolder.requiredArea, experienceContainerHolder.palceOn, experienceContainerHolder.hasExtra, up);
    }
}