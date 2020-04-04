using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BundleStateHandler : MonoBehaviour
{
    [SerializeField] string bundleName;
    [SerializeField] string id;
    [SerializeField] Button comicBtn;
    [SerializeField] Sprite comicBtnSp;
    [SerializeField] List<Image> tokenImgs;
    [SerializeField] List<ExperienceToken> experienceTokens;
    [SerializeField] ComicStateHandler comic;
    [SerializeField] BundleHandler handler;
    bool isComicActive;
    public string BundleName { get => bundleName; }
    public string Id { get => id; set { id = value; handler.BundleID = value; } }
    public void ActiveToken(string _TokenName) {
        bool isAllActive = true;
        for (int i = 0; i < experienceTokens.Count; i++) {
            if (experienceTokens[i].tokenName == _TokenName) {
                experienceTokens[i].isCollected = true;
                tokenImgs[i].sprite = experienceTokens[i].tokenSprite;
            }
            isAllActive &= experienceTokens[i].isCollected;
        }
        if (isAllActive) {
            comicBtn.image.sprite = comicBtnSp;
            isComicActive = true;
            comic.ActiveComic();
        }
    }
    private void Start()
    {
        ActiveToken(string.Empty);
    }
    public void PlayComic() {
        if (!isComicActive)
            return;
        comic.PlayComic();
    }
    public int GetTokensNumber() => experienceTokens.Count;


}
