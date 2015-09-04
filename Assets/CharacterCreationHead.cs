using UnityEngine;
using System.Collections;
using System;
using InventoryQuest.Components.Entities;
using UnityEngine.UI;

public class CharacterCreationHead : MonoBehaviour
{
    public Sprite[] Heads;

    public float SeparationDistance = 10;
    public float CatchSpeed = 200f;

    public GameObject TutorialPlayerHead;

    private bool isSlowing = false;

    private ScrollRect _scrollRect;
    private RectTransform _rectTransform;

    public Sprite CurrentHead { get; set; }

    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _scrollRect = transform.parent.GetComponent<ScrollRect>();
        float posCounter = -(((Heads.Length - 1) * (100 + SeparationDistance)) / 2f) - (100 + SeparationDistance);
        for (int i = 0; i < Heads.Length; i++)
        {
            var item = Heads[i];
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
                CurrentHead = Heads[(int)(-_rectTransform.anchoredPosition.x / (100 + SeparationDistance)) + Mathf.FloorToInt(Heads.Length / 2f)];
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
}
