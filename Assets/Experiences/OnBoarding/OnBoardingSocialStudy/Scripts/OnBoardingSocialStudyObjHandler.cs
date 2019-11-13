using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingSocialStudyObjHandler : MonoBehaviour
{
    [SerializeField] Canvas hotspotCanvas;
    [SerializeField] GameObject[] puzzles;
    public void ShowHotSpot() {
        hotspotCanvas.worldCamera = interactions.Instance.SessionOrigin.camera;
        hotspotCanvas.gameObject.SetActive(true);
    }
    public void HotSpotHit() {
        for (int i = 0; i < puzzles.Length; i++)
        {
            puzzles[i].SetActive(true);
        }
        hotspotCanvas.gameObject.SetActive(true);
    }
}
