﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using InventoryQuest.Components.Entities;
using InventoryQuest.Game;

public class Enemy : MonoBehaviour, IPointerClickHandler
{

    public const float MaxCombatPosition = 241f;
    public const float MinCombatPosition = 94f;

    public bool IsRightSide = true;
    public int EntityID;

    private float _progress;
    private Entity _entityData;
    private RectTransform _rectTransform;

    public Entity EntityData
    {
        get { return _entityData; }
    }

    // Use this for initialization
    void Start()
    {
        _entityData = CurrentGame.Instance.FightController.Enemy[EntityID];
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _progress = EntityData.Position;
        float position = 0;
        if (_progress < 0)
        {
            position = _progress - MinCombatPosition;
            if (MaxCombatPosition < Mathf.Abs(position))
            {
                position = -MaxCombatPosition;
            }
        }
        else
        {
            position = _progress + MinCombatPosition;
            if (MaxCombatPosition < Mathf.Abs(position))
            {
                position = MaxCombatPosition;
            }
        }

        _rectTransform.anchoredPosition = new Vector2(position, 0);
        if (EntityData.Stats.HealthPoints.Current <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CurrentGame.Instance.FightController.Attack(CurrentGame.Instance.Player, EntityData);
    }
}
