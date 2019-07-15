using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] SoundEvent soundEvent;
    [SerializeField] SoundEvent soundEvent2;
    [SerializeField] GameEvent changeSoundToHalf;
    [SerializeField] GameEvent changeSoundToOne;

    public void PlaySound(string s1, string s2)
    {
        soundEvent.Raise(s1, s2);
    }

    public void ChangeSoundToHalf()
    {
        changeSoundToHalf.Raise();
    }
    public void ChangeSoundToOne()
    {
        changeSoundToOne.Raise();
    }
    public void ChangeVolume(float vol, string s2)
    {
        soundEvent.Raise(vol, s2);
    }
     
}
