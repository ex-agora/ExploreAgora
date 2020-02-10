using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Player Info", menuName ="SO/Player/PlayerInfo",order =0)]
public class PlayerProfile : ScriptableObject
{
    #region Fields
    [SerializeField] Countries country;
    [SerializeField] UDateTime date;
    [SerializeField] string firstName;
    [SerializeField] Gender gender;
    [SerializeField] string lastName;
    [SerializeField] string username;
    #endregion Fields
}
