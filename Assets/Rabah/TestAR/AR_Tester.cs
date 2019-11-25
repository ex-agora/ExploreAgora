using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Tester : MonoBehaviour
{
    [SerializeField] Camera aR_TesterCamera;
    [SerializeField] interactions interactions;
    [SerializeField] Transform planeTarget;
    // Start is called before the first frame update
    void Start ()
    {
        aR_TesterCamera.transform.localPosition = Vector3.up;
        aR_TesterCamera.transform.localEulerAngles = new Vector3 (90 , 0 , 0);
        interactions.canSet = true;
        planeTarget.localPosition = Vector3.zero;
        Invoke (nameof(StartARDetection) , 2);
    }

    void StartARDetection ()
    {
        aR_TesterCamera.transform.localPosition = new Vector3 (0 , 0 , -1);
        aR_TesterCamera.transform.localEulerAngles = Vector3.zero;
    }
}
