using System;
using UnityEngine;
using System.Collections;
using InventoryQuest;
using InventoryQuest.Components.Items;
using InventoryQuest.Utils;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //0 index is dedicated to background clip
    public AudioClip BackgroundClips;

    private AudioSource[] _audioSources;

    void Awake()
    {
        _audioSources = GetComponents<AudioSource>();
        Instance = this;
        _audioSources[0].clip = BackgroundClips;
        _audioSources[0].Play();
    }

    public void Play(AudioClip clip)
    {
        var audioSource = GetAudioSource();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Play(Item itemData, EnumItemSoundType soundType)
    {
        if (itemData.SoundID == null || itemData.SoundID.Count < Enum.GetNames(typeof(EnumItemSoundType)).Length)
        {
            Debug.LogWarning("Sound is not connected to item " + itemData.Name + " at " + soundType);
            return;
        }

        var soundItem = itemData.SoundID[(int)soundType];
        if (soundItem != null)
        {
            var path =
                ResourcesNames.ItemsSoundsNames[soundItem.ImageIDType].FullNameList[
                    soundItem.ImageIDItem];
            var clip = (AudioClip)Resources.Load(path);
            if (clip != null)
            {
                var audioSource = GetAudioSource();
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Sound " + soundItem.ImageIDType + " " + soundItem.ImageIDItem + " does not exists");
            }
        }
        else
        {
            Debug.LogWarning("(L)Sound is not connected to item " + itemData.Name + " at " + soundType);
        }
    }

    /// <summary>
    /// Get not playing audio source, or if all are bussy return last one
    /// </summary>
    /// <returns></returns>
    private AudioSource GetAudioSource()
    {
        for (int index = 1; index < _audioSources.Length; index++)
        {
            var audioSource = _audioSources[index];
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return _audioSources[_audioSources.Length - 1];
    }
}
