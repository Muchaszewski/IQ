﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingTextAnimation : MonoBehaviour
{
    public float TimeForAnimation = 1f;
    public float TimeForAlphaFade = 0.5f;
    public float TimeForTurn = 0.3f;
    public float AnimationHeight = 100;
    public float AnimationMaxWidth = 70;
    public float AnimationMinWidth = 50;
    public bool IsRandomizingSide = true;
    [Tooltip("Starting direciton of movement (can be used from floating text class)")]
    public float Direction = 0;

    public Color ColorDamage;
    public Color ColorMissed;
    public Color ColorCritical;
    public Color ColorParried;
    public Color ColorBlocked;
    public Color ColorAbsorbed;
    public Color ColorExhausted;
    public Color ColorExperience;

    //[Tooltip("If enemy is on the left side, floating text will float to the left, and vice versa. Middle will only float upwards")]
    //public bool IsDependOnEntitySize = true;
    public bool IsIncreasingInSize = true;
    public float TimeToSizeUp = 0.4f;

    private Text _text;
    private RectTransform _rectTransform;
    private Vector3 _initialSize;
    private Outline _outline;

    private float _currentAnimationTime = 0f;
    private float _currentWidthMod;

    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<Text>();
        _outline = GetComponent<Outline>();
        _initialSize = transform.localScale;
        if (IsIncreasingInSize)
        {
            transform.localScale = Vector3.zero;
        }
        _currentWidthMod = Random.Range(AnimationMinWidth, AnimationMaxWidth);
    }

    // Update is called once per frame
    void Update()
    {

        // Increase size
        _currentAnimationTime += Time.deltaTime;
        if (IsIncreasingInSize)
        {
            if (transform.localScale != _initialSize)
            {
                transform.AddScale(new Vector3((_initialSize.x / TimeToSizeUp) * _currentAnimationTime, (_initialSize.y / TimeToSizeUp) * _currentAnimationTime));
                if (transform.localScale.x >= _initialSize.x)
                {
                    IsIncreasingInSize = false;
                    transform.localScale = _initialSize;
                }
            }
        }

        // Move vertically
        var vector2 = new Vector2(0, AnimationHeight / (TimeForAnimation * 100f));
        // Move sideways
        if (TimeForTurn <= _currentAnimationTime)
        {
            var mod = (_currentWidthMod / (TimeForAnimation - TimeForTurn == 0 ? 1 : TimeForAnimation - TimeForTurn)) * Time.deltaTime;
            mod = IsRandomizingSide ? mod : TransformUtils.RandomBool() ? mod : -mod;
            vector2.x = mod;
        }
        transform.position = transform.position + Quaternion.Euler(0, 0, Direction) * vector2;


        // Reduce Alpha
        if (_currentAnimationTime >= TimeForAlphaFade)
        {
            var alphaMod = (1 / (TimeForAnimation - TimeForAlphaFade)) * Time.deltaTime;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - alphaMod);
            _outline.effectColor = new Color(_outline.effectColor.r, _outline.effectColor.g, _outline.effectColor.b, _outline.effectColor.a - alphaMod);
        }



        // Destroy
        if (TimeForAnimation <= _currentAnimationTime)
        {
            Destroy(gameObject);
        }
    }
}
