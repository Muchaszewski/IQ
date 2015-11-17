using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components;
using InventoryQuest.Game;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public float TimeToPreloadClip = 0.2f;

    public AudioClip[] PreLoadedClips;
    public Sprite[] PreLoadedImages;

    private float _timer = 0;
    private AsyncOperation _operation;
    private bool _areSoundPrecached = false;
    private bool _areImagesPrecached = false;

    int _sceneTemp = 0;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        var load = GenerationStorage.Instance.Spots[0];
        StartCoroutine(PreCacheAudio());
        StartCoroutine(PreCacheImages());
        LoadScene();
    }

    IEnumerator PreCacheAudio()
    {
        foreach (var clip in PreLoadedClips)
        {
#if UNITY_EDITOR
            Debug.Log("AudioClip: " + clip.name);
#endif
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
            while (true)
            {
                _timer += Time.deltaTime;
                if (_timer >= TimeToPreloadClip)
                {
                    _timer = 0;
                    break;
                }
                else
                {
                    yield return null;
                }
            }
            yield return null;
        }
        Destroy(GetComponent<AudioSource>());
        _areSoundPrecached = true;
    }

    IEnumerator PreCacheImages()
    {
        foreach (var sprite in PreLoadedImages)
        {
#if UNITY_EDITOR
            Debug.Log("Image: " + sprite.name);
#endif
            GetComponent<Image>().sprite = sprite;
            yield return null;
        }
        Destroy(GetComponent<Image>());
        _areImagesPrecached = true;
    }

    void LoadScene()
    {
        _operation = Application.LoadLevelAsync("MainScene");
        _operation.allowSceneActivation = false;
    }

    void Update()
    {
        if (Application.loadedLevel == 0)
        {
            if (_operation != null)
            {
#if UNITY_EDITOR
                Debug.Log("Scene: " + _operation.progress);
#endif
                if (_areSoundPrecached && _areImagesPrecached)
                {
                    _operation.allowSceneActivation = true;
                }
            }
        }
    }

}
