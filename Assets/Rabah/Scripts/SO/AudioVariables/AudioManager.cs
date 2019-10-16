
/// <summary>
/// AudioManager class.
/// </summary>
/// <remarks>
/// <para>This class can Play, ToggleMute, ChangeVolume, ChangeVolumeToHalf and ChangeVolumeToOne.</para>
/// <para> All functions take a parameter called (sfxName) is one of the three audio sources in audio manager prefab(Background, UI, Activity)</para>
/// </remarks>
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// AudioSources from audio manager prefab
    /// </summary>
    [Header("Audio Source")]
    [SerializeField] AudioSource backgroundSource;
    [SerializeField] AudioSource uiSource;
    [SerializeField] AudioSource activitySource;

    /// <summary>
    /// AudioClipsContainers are ScriptableObject contains dictionary of clip name and audio clip
    /// </summary>
    [Header("Audio Clips Container")]
    [SerializeField] AudioClipsContainer backgroundContainer;
    [SerializeField] AudioClipsContainer uiContainer;
    [SerializeField] AudioClipsContainer activityContainer;

    static AudioManager instance;

    AudioSource tempAudioSource;
    AudioClipsContainer tempAudioContainer;
    AudioClip tempAudioClip;

    //-----------------------------------------------------------------------
    public int audioCounter = 0;
    public List<AudioSource> backgroundSources = new List<AudioSource>();
    public List<AudioSource> sfxSources = new List<AudioSource>();
    //-----------------------------------------------------------------------

    /// <value>static instance of AudioManager to access it easily</value>
    public static AudioManager Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        backgroundSources.Add(backgroundSource);
        sfxSources.Add(uiSource);
        sfxSources.Add(activitySource);
    }

    /// <summary>
    /// Play the sound and needs two parameters audioClipName, sfxName
    /// Create a new Audio Clips Container event scriptable object and 
    /// </summary>
    /// <param name="audioClipName">the audio clip name</param>
    /// <param name="sfxName">audio source game object name</param>
    public void Play(string audioClipName, string sfxName)
    {
        if (!HandleAudioSource(sfxName)) return;

        if (tempAudioContainer.sfx.TryGetValue(audioClipName, out tempAudioClip))
        {
            if (tempAudioSource.isPlaying)
                tempAudioSource.Stop();

            tempAudioSource.clip = tempAudioClip;
            tempAudioSource.Play();
        }
    }
    /// <summary>
    /// Mute/UnMute the sound and needs sfxName
    /// </summary>
    /// <param name="sfxName">audio source game object name</param>
    public void ToggleMute(string sfxName)
    {
        if (!HandleAudioSource(sfxName)) return;
        tempAudioSource.mute = !tempAudioSource.mute;
    }
    /// <summary>
    /// Change sound volume and needs two parameters vol, sfxName
    /// </summary>
    /// <param name="vol">new volume</param>
    /// <param name="sfxName">audio source game object name</param>
    public void ChangeVolume(float vol, string sfxName)
    {
        if (!HandleAudioSource(sfxName)) return;
        tempAudioSource.volume = vol;
    }
    /// <summary>
    /// Change sound volume to half and needs sfxName
    /// </summary>
    /// <param name="sfxName">audio source game object name</param>
    public void ChangeVolumeToHalf(string sfxName)
    {
        ChangeVolume(0.5f, sfxName);
    }
    /// <summary>
    /// Change sound volume to one(the maximum volume) and needs sfxName
    /// </summary>
    /// <param name="sfxName">audio source game object name</param>
    public void ChangeVolumeToOne(string sfxName)
    {
        ChangeVolume(1, sfxName);
    }
    /// <summary>
    /// Set the selected Audio source and audio container
    /// </summary>
    /// <returns>
    /// true if sfxName one of  Background, UI, Activity
    /// false if sfxName is not valid
    /// </returns>
    /// <param name="sfxName"></param>
    private bool HandleAudioSource(string sfxName)
    {
        switch (sfxName)
        {
            case "Background":
                tempAudioSource = backgroundSource;
                tempAudioContainer = backgroundContainer;
                break;
            case "UI":
                tempAudioSource = uiSource;
                tempAudioContainer = uiContainer;
                break;
            case "Activity":
                tempAudioSource = activitySource;
                tempAudioContainer = activityContainer;
                break;
            default:
                return false;
        }
        return true;
    }

    public void AudioController(int counterIncrement = -1)
    {
        audioCounter = (audioCounter + 1) % 4;
        audioCounter = counterIncrement == -1 ? audioCounter : counterIncrement;
        switch (audioCounter)
        {
            case 0: // On all.
                for (int i = 0; i < backgroundSources.Count; i++)
                {
                    backgroundSources[i].mute = false;
                }
                for (int i = 0; i < sfxSources.Count; i++)
                {
                    sfxSources[i].mute = false;
                }
                break;
            case 1: // Sound effects only.
                for (int i = 0; i < backgroundSources.Count; i++)
                {
                    backgroundSources[i].mute = true;
                }
                for (int i = 0; i < sfxSources.Count; i++)
                {
                    sfxSources[i].mute = false;
                }
                break;
            case 2: // Backgrounds only.
                for (int i = 0; i < backgroundSources.Count; i++)
                {
                    backgroundSources[i].mute = false;
                }
                for (int i = 0; i < sfxSources.Count; i++)
                {
                    sfxSources[i].mute = true;
                }
                break;
            case 3: // Off all.
                for (int i = 0; i < backgroundSources.Count; i++)
                {
                    backgroundSources[i].mute = true;
                }
                for (int i = 0; i < sfxSources.Count; i++)
                {
                    sfxSources[i].mute = true;
                }
                break;
        }
    }
}