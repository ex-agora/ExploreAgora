using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingSocialStudyObjHandler : MonoBehaviour
{
    [SerializeField] Canvas hotspotCanvas;
    [SerializeField] OldDragable[] puzzles;
    [SerializeField] FadeInOut coalFadding;
    [SerializeField] ParticleSystem effect;

    private void Start()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].enabled = false;
        }

    }
    public void ShowHotSpot()
    {
        hotspotCanvas.worldCamera = interactions.Instance.SessionOrigin.camera;
        hotspotCanvas.gameObject.SetActive(true);
    }
    public void HotSpotHit()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].gameObject.SetActive(true);
            puzzles[i].enabled = true; 
        }
        hotspotCanvas.gameObject.SetActive(false);
    }
    public void CorrtectPuzzle()
    {
        for (int i = 0; i < puzzles.Length - 1; i++)
        {
            puzzles[i].gameObject.SetActive(false);
        }
        effect.gameObject.SetActive(true);
        effect.Play();
        coalFadding.gameObject.SetActive(true);
        coalFadding.fadeInOut(true);
    }

}
