using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132GameManager : MonoBehaviour
{
    #region singletone
    static MSS132GameManager instance;
    public static MSS132GameManager Instance { get => instance; set => instance = value; }
    public SummaryHandler Summary { get => summary; set => summary = value; }
    public MSS132BarHandler BarHandler { get => barHandler; set => barHandler = value; }
    #endregion
    private void Awake ()
    {
        Instance = this;
    }
    [SerializeField] MSS132BarHandler barHandler;
    [SerializeField] SummaryHandler summary;
}
