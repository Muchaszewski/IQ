using UnityEngine;
using System.Collections;
using InventoryQuest;
using InventoryQuest.Utils;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    private AudioSource _audioSource;
    private AudioClip _clip;

    void OnAwake()
    {
        _audioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    public void Play(ImageIDPair soundItem)
    {
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
        }
        else
        {
            Debug.LogWarning("Sound " + soundItem.ImageIDType + " "  + soundItem.ImageIDItem + " does not exists");
        }
    }
}
