using System;
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

    // Use this for initialization
    void Start()
    {
        _entityData = CurrentGame.Instance.FightController.Enemy[EntityID];
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _progress = Mathf.Abs(_entityData.Position);
        Debug.Log(_progress);
        var position = _progress + MinCombatPosition;
        if (MaxCombatPosition < Mathf.Abs(position))
        {
            position = MaxCombatPosition;
        }
        _rectTransform.anchoredPosition = new Vector2(position, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CurrentGame.Instance.FightController.Attack(CurrentGame.Instance.Player, _entityData);
        Debug.Log("Attacked :D");
    }
}
