using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ranks Holder", menuName = "SO/App/Profile/RanksHolder", order = 0)]
public class RanksHolder : ScriptableObject
{
    [SerializeField] StringRankRangeDictionary ranks;
}
