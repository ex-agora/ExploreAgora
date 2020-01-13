using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSS132BulbLightHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] Light bulbLight;
    [SerializeField] MeshRenderer bulbMesh;
    Color currentColor;
    float defaultIntensity;
    float defaultMatAlpha;
    [SerializeField] float turnOnIntensity;
    [SerializeField] float turnOnMaterialAlpha;
    #endregion Fields

    #region Methods
    public void turnOffLight()
    {
        currentColor = bulbMesh.materials[0].color;
        currentColor.a = defaultMatAlpha;
        bulbMesh.materials[0].color = currentColor;
        bulbLight.intensity = defaultIntensity;
    }

    public void turnOnLight()
    {
        currentColor = bulbMesh.materials[0].color;
        currentColor.a = turnOnMaterialAlpha;
        bulbMesh.materials[0].color = currentColor;
        bulbLight.intensity = turnOnIntensity;
    }

    // Start is called before the first frame update
    void Start ()
    {
        defaultMatAlpha = bulbMesh.materials[0].color.a;
        defaultIntensity = bulbLight.intensity;
    }
    #endregion Methods
}
