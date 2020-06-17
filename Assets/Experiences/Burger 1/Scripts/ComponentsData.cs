using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsData : MonoBehaviour
{
    [SerializeField] ESandwichComponents sandwichComponents;
    [SerializeField] GameObject myReleventComponent;

    public ESandwichComponents SandwichComponents { get => sandwichComponents; set => sandwichComponents = value; }
    public GameObject MyReleventComponent { get => myReleventComponent; set => myReleventComponent = value; }

    
}
