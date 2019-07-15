using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioClipsContainer", menuName = "SO/Variable/AudioClipsContainer", order = 0)]
public class AudioClipsContainer : ScriptableObject
{
    public StringAudioClipsDictionary sfx;
}
