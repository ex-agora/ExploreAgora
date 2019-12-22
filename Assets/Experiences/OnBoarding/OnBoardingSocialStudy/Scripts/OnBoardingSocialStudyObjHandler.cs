using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingSocialStudyObjHandler : MonoBehaviour
{
    [SerializeField] Canvas hotspotCanvas;
    [SerializeField] OldDragable[] puzzles;
    [SerializeField] FadeInOut coalFadding;
    [SerializeField] ParticleSystem effect;
    [SerializeField] GameEvent finalPlaceEvent = null;
    [SerializeField] GameEvent coalShowEvent = null;
    int counter;
    private void Awake()
    {
        counter = 0;
    }
    private void OnEnable()
    {
        counter++;
        if (counter == 2)
        {
            finalPlaceEvent?.Raise();
            ShowHotSpot();
        }
    }
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
        AudioManager.Instance.Play("placeObject", "Activity");
        effect.gameObject.SetActive(true);
        effect.Play();
        coalFadding.gameObject.SetActive(true);
        coalFadding.fadeInOut(true);
        coalShowEvent?.Raise();
    }
    public void StopVFX() {
        effect.Stop();
        effect.gameObject.SetActive(false);
    }

}
