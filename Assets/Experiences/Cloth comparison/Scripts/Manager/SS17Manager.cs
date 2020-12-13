using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS17Manager : MonoBehaviour
{
    static SS17Manager instance;
    Mannequin blue;
    Mannequin purple;
    public static SS17Manager Instance { get => instance; set => instance = value; }
    public Mannequin Blue { get => blue; set => blue = value; }
    public Mannequin Purple { get => purple; set => purple = value; }

    #region Functions
    #region Game
    //Compare Button
    public void Match ()
    {
        if ( blue.MatchedMannequin == purple )
        {
            if ( !blue.IsMatched )
            {
                blue.RelationImageBarEvent?.Raise ();
                SS17GameManager.Instance.OpenMidSummery (blue.MidSummary);
                SS17GameManager.Instance.CloseGotItBtn ();
                blue.IsMatched = true;
                purple.IsMatched = true;
            }
        }
        else
        {
            SS17GameManager.Instance.TryAgain ();
        }
    }
    public void ActiveMatchButton ()
    {
        if ( blue && purple )
        {
            SS17GameManager.Instance.MatchButton.interactable = true;
        }
    }

    #endregion
    #region Mono
    private void Awake ()
    {
        if ( Instance == null )
            Instance = this;
    }
    // Start is called before the first frame update
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }
    #endregion
    #endregion
}
