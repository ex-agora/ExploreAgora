using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour
{
    [SerializeField] Mannequin matchedMannequin;
    [SerializeField] GameEvent relationImageBarEvent;
    [SerializeField] RectTransform hotspotPos;
    [SerializeField] SS17MidSummary midSummary;
    bool isMatched;
    public Mannequin MatchedMannequin { get => matchedMannequin; set => matchedMannequin = value; }
    public GameEvent RelationImageBarEvent { get => relationImageBarEvent; set => relationImageBarEvent = value; }
    public RectTransform HotspotPos { get => hotspotPos; set => hotspotPos = value; }
    public SS17MidSummary MidSummary { get => midSummary; set => midSummary = value; }
    public bool IsMatched { get => isMatched; set => isMatched = value; }
}
