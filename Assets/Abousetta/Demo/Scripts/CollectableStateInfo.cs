using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableStateInfo : MonoBehaviour
{
    #region Fields
    string collectableName;
    [SerializeField] Sprite collectableSprite;
    string description;
    #endregion Fields

    #region Properties
    public string CollectableName { get => collectableName; set => collectableName = value; }
    public Sprite CollectableSprite { get => collectableSprite; set => collectableSprite = value; }
    public string Description { get => description; set => description = value; }
    #endregion Properties
}
