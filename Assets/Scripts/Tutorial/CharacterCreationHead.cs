using UnityEngine;
using System.Collections;
using System;
using InventoryQuest.Components.Entities;
using UnityEngine.UI;

public class CharacterCreationHead : MonoBehaviour
{
    public Sprite[] HeadsMale;
    public Sprite[] HeadsFemale;

    public float SeparationDistance = 10;
    public float CatchSpeed = 200f;

    public GameObject TutorialPlayerHead;

    public Slider SexSlider;

    private bool isSlowing = false;
    private Sprite[] CurrentHeads;

    private ScrollRect _scrollRect;
    private RectTransform _rectTransform;

    public Sprite CurrentHead { get; set; }

    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _scrollRect = transform.parent.GetComponent<ScrollRect>();
        CurrentHeads = HeadsMale;
        ShowHeads();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSlowing || Mathf.Abs(_scrollRect.velocity.x) <= CatchSpeed)
        {
            var headPercent = _rectTransform.anchoredPosition.x % (100 + SeparationDistance);
            if (Mathf.Abs(headPercent) <= 10)
            {
                _scrollRect.velocity = new Vector2(0, 0);
                _rectTransform.anchoredPosition = new Vector2((int)(_rectTransform.anchoredPosition.x / (100 + SeparationDistance)) * (100 + SeparationDistance), 0);
                isSlowing = false;
                var tt = (int)(-_rectTransform.anchoredPosition.x / (100 + SeparationDistance)) + Mathf.FloorToInt(CurrentHeads.Length / 2f);
                if (tt < 0 || tt > CurrentHeads.Length - 1)
                {
                    return;
                }
                CurrentHead = CurrentHeads[tt];
                return;
            }
            if (_scrollRect.velocity.x <= 60 && _scrollRect.velocity.x >= 0)
            {
                _scrollRect.velocity = new Vector2(60f, 0);
            }
            if (_scrollRect.velocity.x >= -60 && _scrollRect.velocity.x <= 0)
            {
                _scrollRect.velocity = new Vector2(-60f, 0);
            }
        }
    }

    void ShowHeads()
    {
        _rectTransform.sizeDelta = new Vector2(576, 0);
        float posCounter = -(((CurrentHeads.Length - 1) * (100 + SeparationDistance)) / 2f) - (100 + SeparationDistance);
        for (int i = 0; i < CurrentHeads.Length; i++)
        {
            var item = CurrentHeads[i];
            var head = Instantiate(TutorialPlayerHead);
            head.transform.SetParent(this.transform);
            head.transform.localScale = Vector3.one;
            head.GetComponent<Image>().sprite = item;
            if (i == 0)
            {
                head.GetComponent<RectTransform>().anchoredPosition = new Vector2(posCounter + 100 + SeparationDistance, 0);
                posCounter += 100 + SeparationDistance;
            }
            else
            {
                head.GetComponent<RectTransform>().anchoredPosition = new Vector2(posCounter + 100 + SeparationDistance, 0);
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x + 100 + SeparationDistance, _rectTransform.sizeDelta.y);
                posCounter += 100 + SeparationDistance;
            }
        }
    }

    public void ChangeSex()
    {
        if (SexSlider.value == 0)
        {
            CurrentHeads = HeadsMale;
        }
        else
        {
            CurrentHeads = HeadsFemale;
        }
        ShowHeads();
    }
}
