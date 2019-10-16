using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Player Info", menuName ="SO/Player/PlayerInfo",order =0)]
public class PlayerProfile : ScriptableObject
{
    [SerializeField] string firstName;
    [SerializeField] string lastName;
    [SerializeField] string username;
    [SerializeField] Gender gender;
    [SerializeField] Countries country;
    [SerializeField] UDateTime date;
}
