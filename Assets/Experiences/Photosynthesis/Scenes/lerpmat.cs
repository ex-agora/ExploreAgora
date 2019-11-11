using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class lerpmat : MonoBehaviour
{
    // Blends between two materials

    [SerializeField] Material material1;
    [SerializeField] Material material2;
    float duration = 2.0f;
    Renderer rend;

    void Start ()
    {
        rend = GetComponent<Renderer> ();
        // At start, use the first material
        rend.material = material1;
    }

    void Update ()
    {
        float lerp = Mathf.PingPong (Time.time, duration) / duration;
        // ping-pong between the materials over the duration
        rend.material.Lerp (material1, material2, lerp);
    }
}