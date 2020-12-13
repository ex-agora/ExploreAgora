using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEvent", menuName = "SO/Variable/SoundEvent", order = 0)]
public class SoundEvent : ScriptableObject
{
 public void Raise(string audioClipName , string sfxName)
    {
        AudioManager.Instance.Play(audioClipName , sfxName);
    }

    public void Raise(float vol, string sfxName)
    {
        AudioManager.Instance.ChangeVolume(vol , sfxName);
    }
}
