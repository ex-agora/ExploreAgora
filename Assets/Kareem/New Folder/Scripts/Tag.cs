using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Tag", menuName = "SO/Experience/Tag", order = 0)]
public class Tag : ScriptableObject
{
    public List<string> tags;
}