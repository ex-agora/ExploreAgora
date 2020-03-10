using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StoryHandler : MonoBehaviour
{
    [SerializeField] List<Sprite> stories;
    [SerializeField] Image storyViewer;
    [SerializeField] Sprite finalStorySprite;
    [SerializeField] Sprite startStorySprite;
    [SerializeField] GameObject storyObj;
    [SerializeField] GameObject openStoryBtnObj;
    [SerializeField] TMPro.TMP_Text currentStoryIndextText;
    [SerializeField] UnityEvent startStoriesEvent;
    [SerializeField] UnityEvent endStoriesEvent;
    //[SerializeField] StoriesProgressBar progressBar;
    public List<Sprite> Stories { get => stories; set => stories = value; }
    public int StoriesIndex { get => storiesIndex; set => storiesIndex = value; }
    int storiesIndex = -1;
    Sprite currentStory;

    public void GetNextStory ()
    {        
        if ( storiesIndex < stories.Count -1 )
        {
            StoriesIndex++;
            currentStory = Stories [StoriesIndex];
            ShowStory (currentStory);
            //progressBar.ActivateStory (StoriesIndex);
        }
        else
        {
            EndStories ();
        }
    }
    public void GetPreviousStory ()
    {
        if ( storiesIndex > 0 )
        {
            //progressBar.DeactivateStory (StoriesIndex);
            StoriesIndex--;
            currentStory = Stories [StoriesIndex];
            ShowStory (currentStory);
        }
    }
    void ShowStory (Sprite story)
    {
        currentStoryIndextText.text = "" + StoriesIndex;
        storyViewer.sprite = story;
    }
    void EndStories ()
    {
        endStoriesEvent.Invoke ();
    }
    public void TestEndStories ()
    {
        currentStoryIndextText.text = "";
        storyViewer.sprite = finalStorySprite;
        HideStories ();
        Debug.Log ("5lsna ya amir");
    }
    public void TestStartStories ()
    {
        storyViewer.sprite = startStorySprite;
        currentStoryIndextText.text = "";
    }

    void HideStories ()
    {
        storyObj.SetActive (false);
        //openStoryBtnObj.SetActive (true);
    }
    public void StartStories ()
    {
        currentStory = null;
        StoriesIndex = 0;
        startStoriesEvent.Invoke ();
    }
}
