using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoriesProgressBar : MonoBehaviour
{
    [SerializeField] Color seenStoryColor;
    [SerializeField] Color unSeenStoryColor;
    [SerializeField] RectTransform progressStoryImage;
    [SerializeField] StoryHandler storyHandler;
    [SerializeField] float maxStoriesPanelWidth = 800;
    float progressStoryImagesCounter;
    float storyWidth;
    Vector2 storySizeDelta;
    Vector2 progressImagePos;
    List<Image> storiesBar;
    // Start is called before the first frame update
    void Start ()
    {

        storiesBar = new List<Image> ();
        progressStoryImagesCounter = storyHandler.Stories.Count;
        //storyWidth = maxStoriesPanelWidth / progressStoryImagesCounter;
        //progressImagePos.x = storyWidth;
        //progressImagePos.y = storyWidth;
        //storySizeDelta = progressImagePos;
        for ( int i = 0 ; i < progressStoryImagesCounter ; i++ )
        {
            RectTransform progressImage = Instantiate (progressStoryImage , this.transform);
            //progressImage.localPosition =
            progressImage.sizeDelta = new Vector2(35,35);
             //   new Vector2 (progressImage.localPosition.x + ( i * storyWidth ) , progressImage.localPosition.y);
            storiesBar.Add (progressImage.GetComponent<Image> ());
            storiesBar[i].color = unSeenStoryColor;
        }
    }
    public void ActivateStory (int storyIndex)
    {
        storiesBar [storyIndex].color = seenStoryColor;
    }
    public void DeactivateStory (int storyIndex)
    {
        storiesBar [storyIndex].color = unSeenStoryColor;
    }
    public void ResetStories ()
    {
        for ( int i = 0 ; i < storiesBar.Count ; i++ )
        {
            DeactivateStory (i);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < storiesBar.Count; i++)
         {
            Destroy(storiesBar[i].gameObject);
        }
        storiesBar.Clear();
    }
}
