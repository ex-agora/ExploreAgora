using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicStateHandler : MonoBehaviour
{
    [SerializeField] Image comicBtn;
    [SerializeField] Sprite activeBtnSp;
    [SerializeField] Image comicBG;
    [SerializeField] Sprite activeBGSp;
    [SerializeField] StoryHandler story;
    bool isActiveComic;

    public void ActiveComic() { isActiveComic = true; comicBtn.sprite = activeBtnSp; comicBG.sprite = activeBGSp; }
    public void PlayComic() {
        if (!isActiveComic)
            return;
        story.gameObject.SetActive(true);
        story.StartStories();
    }
}
