using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotHandler : MonoBehaviour
{
    HotSpotGroupManager manager;
    bool isCollected;
    bool isOpened;
    [SerializeField] LabelWorldHandler label;
    [SerializeField] Canvas canvas;
    [SerializeField] OutlineStateHandler outline;
    public HotSpotGroupManager Manager { get => manager; set => manager = value; }
    public bool IsOpened { get => isOpened; }
    private void Start()
    {
        canvas.worldCamera = PlantPartsGameManager.Instance.ArCamera;
    }
    public void Tap() {
        Manager.ShowHotSpotLabel(this);
        
    }

    public void Close() {
        isOpened = false;
        outline.HideOutline();
        label.HidaLabel();
    }
    public void Open() {
        //AudioManager.Instance.Play("notification", "Activity");
        Collect();
        isOpened = true;
        outline.ShowOutline();
        label.ShowLabel();
    }
    void Collect() {
        if (!isCollected) {
            isCollected = true;
            //TODO Notify CounDwwwwWt Manager
        }
    }
    public void PrepaireQuiz() {
        label.PrepaireQuiz();
        outline.HideOutline();
        gameObject.SetActive(false);
    }
    public bool HasExtraInfo() => label.HasExtraInfo;
    public string ExtraInfoText ()=> label.ExtraInfoText;
}
