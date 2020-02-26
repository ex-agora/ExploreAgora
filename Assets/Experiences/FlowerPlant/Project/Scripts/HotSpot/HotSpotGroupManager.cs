using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HotSpotGroupManager : MonoBehaviour
{
    [SerializeField] UpperLabelWorldHandler upperLabel;
    [SerializeField] List<HotSpotHandler> spotHandlers;
    HotSpotHandler currentHotSpot;
    HashSet<HotSpotHandler> hotsOpen = new HashSet<HotSpotHandler>();
    [SerializeField] bool isAllOpened;
    [SerializeField] GameEvent hotSpotCounterEvent;
    [SerializeField] TMP_Text text;
    public int HotSpotOpenedCounter { get => hotsOpen.Count; }
    public int HotSpotMaxCounter { get => spotHandlers.Count; }
    bool isDone;
    private void Start()
    {
        for (int i = 0; i < spotHandlers.Count; i++)
        {
            spotHandlers[i].Manager = this;
        }
        PlantPartsGameManager.Instance.AddHotSpotGroupManager(this);    
    }
    public void ShowAllHotSpot() {
        for (int i = 0; i < spotHandlers.Count; i++)
        {
            spotHandlers[i].gameObject.SetActive(true);
        }
        PlantPartsGameManager.Instance.GoToNextBubbleState();
        PlantPartsGameManager.Instance.ShowCounter();
    }
    public void HideAllHotSpot() {
        for (int i = 0; i < spotHandlers.Count; i++)
        {
            spotHandlers[i].gameObject.SetActive(false);
        }
    }
    void CheckOpendAll() {
        isAllOpened = spotHandlers.Count == hotsOpen.Count;
        if (!isDone && isAllOpened) {
            isDone = true;
            PlantPartsGameManager.Instance.CheckExploraState();
        }
    }
    public void ShowHotSpotLabel(HotSpotHandler hotSpot) {
        //if (hotSpot == currentHotSpot)
        //    return;
        //if (!(currentHotSpot is null)) {
        //    currentHotSpot.Close();
        //}
        //currentHotSpot = hotSpot;
        //currentHotSpot.Open();
        //if (currentHotSpot.HasExtraInfo())
        //{
        //    upperLabel.Info = currentHotSpot.ExtraInfoText();
        //    upperLabel.Show();
        //}
        //else {
        //    upperLabel.Hide();
        //}
        if (hotSpot.IsOpened)
        {
            hotSpot.Close();
            if(hotSpot.HasExtraInfo())
                upperLabel.Hide();
        }
        else
        {
            hotSpot.Open();
            if (hotSpot.HasExtraInfo())
            {
                upperLabel.Info = hotSpot.ExtraInfoText();
                upperLabel.Show();
            }
            else
            {
                upperLabel.Hide();
            }
            if (hotsOpen.Add(hotSpot))
                hotSpotCounterEvent.Raise();
            CheckOpendAll();
        }
    }
 
    public void PrepaireQuiz() {
        for (int i = 0; i < spotHandlers.Count; i++)
        {
            spotHandlers[i].PrepaireQuiz();
        }
        upperLabel.Hide();
        gameObject.SetActive(true);
        text.gameObject.SetActive(false);
    }
}
