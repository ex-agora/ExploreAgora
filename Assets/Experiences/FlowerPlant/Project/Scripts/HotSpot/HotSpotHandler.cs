using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotHandler : MonoBehaviour
{
    HotSpotGroupManager manager;
    bool isCollected;
    bool isOpened;
    [SerializeField] LabelWorldHandler label;
    public HotSpotGroupManager Manager { get => manager; set => manager = value; }
    public bool IsOpened { get => isOpened; }

    public void Tap() {
        Manager.ShowHotSpotLabel(this);
    }

    public void Close() {
        isOpened = false;
        label.HidaLabel();
    }
    public void Open() {
        Collect();
        isOpened = true;
        label.ShowLabel();
    }
    void Collect() {
        if (!isCollected) {
            isCollected = true;
            //TODO Notify Count Manager
        }
    }
    public void PrepaireQuiz() {
        label.PrepaireQuiz();
        gameObject.SetActive(false);
    }
    public bool HasExtraInfo() => label.HasExtraInfo;
    public string ExtraInfoText ()=> label.ExtraInfoText;
}
