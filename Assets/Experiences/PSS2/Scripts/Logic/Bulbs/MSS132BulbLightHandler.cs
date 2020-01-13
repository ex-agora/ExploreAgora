using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132BulbLightHandler : MonoBehaviour
{
    [SerializeField] MeshRenderer bulbMesh;
    [SerializeField] Light bulbLight;
    [SerializeField] float turnOnMaterialAlpha;
    [SerializeField] float turnOnIntensity;
    float defaultMatAlpha;
    float defaultIntensity;
    Color currentColor;
    // Start is called before the first frame update
    void Start ()
    {
        defaultMatAlpha = bulbMesh.materials [0].color.a;
        defaultIntensity = bulbLight.intensity;
    }

    public void turnOnLight ()
    {
        currentColor = bulbMesh.materials [0].color;
        currentColor.a = turnOnMaterialAlpha;
        bulbMesh.materials [0].color = currentColor;
        bulbLight.intensity = turnOnIntensity;
    }
    public void turnOffLight ()
    {
        currentColor = bulbMesh.materials [0].color;
        currentColor.a = defaultMatAlpha;
        bulbMesh.materials [0].color = currentColor;
        bulbLight.intensity = defaultIntensity;
    }
}
