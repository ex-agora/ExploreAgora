using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableStateInfo : MonoBehaviour
{
    [SerializeField]Sprite collectableSprite;
    string description;
    string collectableName;

    public Sprite CollectableSprite { get => collectableSprite; set => collectableSprite = value; }
    public string Description { get => description; set => description = value; }
    public string CollectableName { get => collectableName; set => collectableName = value; }

}
