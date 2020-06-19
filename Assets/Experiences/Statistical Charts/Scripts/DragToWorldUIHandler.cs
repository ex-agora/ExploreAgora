using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToWorldUIHandler : MonoBehaviour
{
    private static DragToWorldUIHandler instance;

    [SerializeField] private List<GameObject> buttons;

    public static DragToWorldUIHandler Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void RestoreButton(int _index)
    {
        if (_index >= 0 && _index < buttons.Count)
            buttons[_index]?.SetActive(true);
    }
}