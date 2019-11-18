using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSDistanceAlertMessages : MonoBehaviour
{
    [SerializeField] int minDistanceHintIndex;
    [SerializeField] int maxDistanceHintIndex;
    [SerializeField] List<Draggable> draggables;
    [SerializeField] List<DraggableOnSurface> atoms;
    // Start is called before the first frame update
    void Start ()
    {
        PhotosynthesisGameManager.Instance.Atoms = atoms;
        PhotosynthesisGameManager.Instance.Draggables = draggables;
    }

    // Update is called once per frame
    void Update ()
    {

    }
    public void OnMaxDitance ()
    {
        PhotosynthesisGameManager.Instance.ShowHint (maxDistanceHintIndex);
    }
    public void OnMinDitance ()
    {
        PhotosynthesisGameManager.Instance.ShowHint (minDistanceHintIndex);
    }    
    public void OnCorrectDitance ()
    {
        PhotosynthesisGameManager.Instance.CorrectDistanceFlow();
    }
}
