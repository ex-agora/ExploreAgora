using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotPivotInitializer : MonoBehaviour
{

    public enum currentObjectType
    {
        Transform,
        Particle,
        MeshRenderer,
        Animator,
        Draggable
    }
    //[SerializeField] Transform gameObject;
    //[SerializeField] ParticleSystem particle;
    //[SerializeField] MeshRenderer meshRenderer;

    public currentObjectType ObjectType;
    private void Awake()
    {
        if (ObjectType == currentObjectType.Transform)
        {
            if (transform.name.Contains("Pivot"))
                OnBoardingMathGameManager.Instance.AddHotSpotPivots(transform);
        }
        else if (ObjectType == currentObjectType.Particle)
        {
            if (transform.name.Contains("Powder"))
                OnBoardingMathGameManager.Instance.powderParticleParent = transform;
            else if (transform.name.Contains("Book"))
                OnBoardingMathGameManager.Instance.bookParticleParent = transform;
        }
        else if (ObjectType == currentObjectType.MeshRenderer)
        {
            OnBoardingMathGameManager.Instance.bookMat = GetComponent<SkinnedMeshRenderer>().material;
        }
        else if (ObjectType == currentObjectType.Animator)
            OnBoardingMathGameManager.Instance.BookAnimator = GetComponent<Animator>();

        else if (ObjectType == currentObjectType.Draggable)
            OnBoardingMathGameManager.Instance.AddDraggables(GetComponent<OldDragable>());
    }
}
