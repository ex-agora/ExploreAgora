using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSSBarHandlerEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void CloudButtonEvent ()
    {
        PhotosynthesisGameManager.Instance.CloudHint.GetComponent<PSSUpperButtonsHandler> ().Active ();
    }
    public void AirButtonEvent ()
    {
        PhotosynthesisGameManager.Instance.AirHint.GetComponent<PSSUpperButtonsHandler> ().Active ();
    }
    public void SunButtonEvent ()
    {
        PhotosynthesisGameManager.Instance.SunHint.GetComponent<PSSUpperButtonsHandler> ().Active ();
        PhotosynthesisGameManager.Instance.NightImageEffect.GetComponent<PSSFadeNightImageToLight> ().ResetFadeImg();
    }
}
