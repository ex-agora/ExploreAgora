using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObjHnadler : MonoBehaviour
{
    [SerializeField] MeshRenderer meshTree;
    [SerializeField] Material afterMatTree;
    [SerializeField] ParticleSystem particle;

    public void StratVFX() {
        
        particle.gameObject.SetActive(true);
        particle.Play();
        AudioManager.Instance.Play("factory", "Activity");
        Invoke(nameof(StopVFX), 2f);
    }
    void StopVFX() {
        particle.Stop();
        meshTree.material = afterMatTree;
    }
}
