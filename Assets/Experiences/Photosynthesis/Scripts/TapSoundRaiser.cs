using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSoundRaiser : MonoBehaviour
{
    public void TapSound() {
        AudioManager.Instance?.Play("UIAction", "UI");
    }
}
