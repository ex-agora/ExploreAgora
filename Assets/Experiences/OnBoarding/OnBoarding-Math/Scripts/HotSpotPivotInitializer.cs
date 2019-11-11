using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotPivotInitializer : MonoBehaviour
{

    public enum currentObjectType
    {
        Transform ,
        Particle ,
        MeshRenderer 
    }
    //[SerializeField] Transform gameObject;
    //[SerializeField] ParticleSystem particle;
    //[SerializeField] MeshRenderer meshRenderer;

    public currentObjectType ObjectType;
    private void Awake()
    {
        if (ObjectType == currentObjectType.Transform && transform.name.Contains("Pivot"))
            OnBoardingMathGameManager.Instance.AddHotSpotPivots(transform);
        else if (ObjectType == currentObjectType.Particle && transform.name.Contains("Powder")) {
            OnBoardingMathGameManager.Instance.powderParticleParent = transform;
        }
        else if (ObjectType == currentObjectType.Particle && transform.name.Contains("Book")) {
            OnBoardingMathGameManager.Instance.bookParticleParent = transform;
        }
        else if (ObjectType == currentObjectType.MeshRenderer)
        {
            OnBoardingMathGameManager.Instance.bookMat = GetComponent<SkinnedMeshRenderer>().material;
        }
        else
            OnBoardingMathGameManager.Instance.BookAnimator = GetComponent<Animator>();
    }
}
