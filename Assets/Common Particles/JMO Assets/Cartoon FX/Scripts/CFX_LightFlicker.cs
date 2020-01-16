using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015 Jean Moreno

// Randomly changes a light's intensity over time.

[RequireComponent(typeof(Light))]
public class CFX_LightFlicker : MonoBehaviour
{
    #region Fields
    /// Max intensity will be: baseIntensity + addIntensity
    public float addIntensity = 1.0f;

    // Loop flicker effect
    public bool loop;
	
	// Perlin scale: makes the flicker more or less smooth
	public float smoothFactor = 1f;
    private float baseIntensity;
    private float maxIntensity;
    private float minIntensity;
    #endregion Fields

    #region Methods
    void Awake()
	{
		baseIntensity = GetComponent<Light>().intensity;
	}
	
	void OnEnable()
	{
		minIntensity = baseIntensity;
		maxIntensity = minIntensity + addIntensity;
	}
	
	void Update ()
	{
		GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * smoothFactor, 0f));
    }
    #endregion Methods
}
