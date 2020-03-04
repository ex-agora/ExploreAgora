using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ScannedObjectHandler : MonoBehaviour
{
    [SerializeField] private Image lockedImage;
    [SerializeField] private Sprite unlockedImage;
    [SerializeField] private Text tittle;
    [SerializeField] private string name;

    public string Name
    {
        get => name;
    }

    private void UnlockObject ()
    {
        lockedImage.sprite = unlockedImage;
        tittle.gameObject.SetActive (true);
    }
}