using System;
using UnityEngine;
using System.Collections;
using InventoryQuest;
using InventoryQuest.Components.Items;
using InventoryQuest.Utils;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    private AudioSource _audioSource;
    private AudioClip _clip;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    public void Play(Item itemData, EnumItemSoundType soundType)
    {
        if (itemData.SoundID == null || itemData.SoundID.Count < Enum.GetNames(typeof(EnumItemSoundType)).Length)
        {
            Debug.LogWarning("Sound is not connected to item " + itemData.Name + " at " + soundType);
        }
        var soundItem = itemData.SoundID[(int)soundType];
        if (soundItem != null)
        {
            var path =
                ResourcesNames.ItemsSoundsNames[soundItem.ImageIDType].FullNameList[
                    soundItem.ImageIDItem];
            _clip = (AudioClip)Resources.Load(path);
            if (_clip != null)
            {
                _audioSource.clip = _clip;
                _audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Sound " + soundItem.ImageIDType + " " + soundItem.ImageIDItem + " does not exists");
            }
        }
        else
        {
            Debug.LogWarning("Sound is not connected to item " + itemData.Name + " at " + soundType);
        }
    }
}
