using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ranks Holder", menuName = "SO/App/Profile/RanksHolder", order = 0)]
public class RanksHolder : ScriptableObject
{
    [SerializeField] StringRankRangeDictionary ranks;

    public KeyValuePair<string, RankRange> GetRank(float _points)
    {
        KeyValuePair<string, RankRange> rank = new KeyValuePair<string, RankRange>(string.Empty, null);

        foreach (var i in ranks.Keys)
        {
            if (_points >= ranks[i].min && _points < ranks[i].max)
            {
                rank = new KeyValuePair<string, RankRange>(i, ranks[i]);
                break;
            }
        }
        return rank;
    }
    /*public KeyValuePair<string, RankRange> GetRank(string _rank)
    {
        KeyValuePair<string, RankRange> rank = new KeyValuePair<string, RankRange>(string.Empty, null);
        RankRange rankRange;

        if (ranks.TryGetValue(_rank, out rankRange))
        {
            rank = new KeyValuePair<string, RankRange>(_rank, rankRange);
        }
        return rank;
    }*/
}