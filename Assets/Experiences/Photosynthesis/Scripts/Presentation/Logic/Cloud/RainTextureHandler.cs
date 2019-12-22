using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTextureHandler : MonoBehaviour
{
    [SerializeField] Material rainMat;
    [SerializeField] float rainSpeed = 1;

    public float RainSpeed { get => rainSpeed; set => rainSpeed = value; }
    private void Start ()
    {
        StopRain ();
    }
    public void StartRain ()
    {
        AudioManager.Instance?.Play("rain", "Activity");
        rainMat.SetFloat ("_MoveFactor" , rainSpeed);
        rainMat.SetFloat ("_Cutoff" , 0.7f);
    }
    public void StopRain ()
    {
        rainMat.SetFloat ("_MoveFactor" , 0);
        rainMat.SetFloat ("_Cutoff" , 1.0f);
    }
}
