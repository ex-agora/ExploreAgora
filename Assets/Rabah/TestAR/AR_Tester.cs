using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Tester : MonoBehaviour
{
    #region Fields
    [SerializeField] Transform planeTarget;
    #endregion Fields

    #region Methods
    // Start is called before the first frame update
    void OnEnable ()
    {
        interactions.Instance.SessionOrigin.camera.transform.localPosition = Vector3.up;
        interactions.Instance.SessionOrigin.camera.transform.localEulerAngles = new Vector3 (90 , 0 , 0);
        interactions.Instance.canSet = true;
        planeTarget.localPosition = Vector3.zero;
        Invoke (nameof(StartARDetection) , 2);
    }

    void StartARDetection ()
    {
        interactions.Instance.SessionOrigin.camera.transform.localPosition = new Vector3 (0 , 0 , -1);
        interactions.Instance.SessionOrigin.camera.transform.localEulerAngles = Vector3.zero;
    }
    #endregion Methods
}
