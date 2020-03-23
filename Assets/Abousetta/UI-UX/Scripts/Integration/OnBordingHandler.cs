using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBordingHandler : MonoBehaviour
{
    [SerializeField] GameObject onbordingPanel;
    [SerializeField] StoryHandler story;
    public void StartOnBording() {
        onbordingPanel.SetActive(true);
        story.gameObject.SetActive(true);
        story.StartStories();
    }
   
}
