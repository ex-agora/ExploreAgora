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
    [SerializeField] GameObject activeTxt;
    [SerializeField] GameObject unactiveTxt;
    [SerializeField] bool isStandAlone;
    bool isComicActive;
    public string BundleName { get => bundleName; }
    public string Id { get => id; set { id = value; HandleID(); } }
    void HandleID() {
        if(handler!=null)
            handler.BundleID = id;
    }
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
            if (!isStandAlone) {
            activeTxt.SetActive(true);
            unactiveTxt.SetActive(false);
            comicBtn.image.sprite = comicBtnSp;
            }
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
