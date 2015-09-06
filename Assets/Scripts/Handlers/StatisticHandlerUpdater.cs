﻿using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;

[RequireComponent(typeof(StatisticHandler))]
[DisallowMultipleComponent]
public class StatisticHandlerUpdater : MonoBehaviour
{

    private StatisticHandler _statisticHandler;

    void Start()
    {
        _statisticHandler = GetComponent<StatisticHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_statisticHandler.statType)
        {
            case EnumStatisticHandler.Stat:
                if (_statisticHandler.stat == EnumTypeStat.DPS)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.DPS.ToString();
                    break;
                }
                if (_statisticHandler.stat == EnumTypeStat.Accuracy)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.Accuracy.ToString();
                    break;
                }
                if (_statisticHandler.stat == EnumTypeStat.AttackSpeed)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.AttackSpeed.ToString();
                    break;
                }
                if (_statisticHandler.stat == EnumTypeStat.Deflection)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.Parry.ToString();
                    break;
                }
                if (_statisticHandler.StatReference.GetType() == typeof(StatValueFloat))
                {
                    SetStatText(_statisticHandler.StatReference as StatValueFloat);
                }
                else if (_statisticHandler.StatReference.GetType() == typeof(StatValueInt))
                {
                    SetStatText(_statisticHandler.StatReference as StatValueInt);
                }
                break;
            case EnumStatisticHandler.Target:
                if (_statisticHandler.StatReference == null)
                {
                    _statisticHandler.TextComponent.text = "";
                    try
                    {
                        _statisticHandler.StatReference = CurrentGame.Instance.FightController.Target.Name;
                    }
                    catch { }
                }
                else
                {
                    _statisticHandler.TextComponent.text = _statisticHandler.StatReference.ToString();
                }
                break;
            case EnumStatisticHandler.Special:
                _statisticHandler.ApplySpecial();
                break;
            case EnumStatisticHandler.Entity:
                if (_statisticHandler.entityStatType == EnumPlayerBasics.Level)
                {
                    _statisticHandler.TextComponent.text = "Level " + CurrentGame.Instance.Player.Level;
                }
                break;
            default:
                _statisticHandler.TextComponent.text = _statisticHandler.StatReference.ToString();
                break;
        }
    }

    void SetStatText<T>(IStatValue<T> stat)
    {
        switch (_statisticHandler.value)
        {
            case EnumStatValue.Extend:
                _statisticHandler.TextComponent.text = stat.Extend.ToString();
                break;
            case EnumStatValue.Base:
                _statisticHandler.TextComponent.text = stat.Base.ToString();
                break;
            case EnumStatValue.Current:
                _statisticHandler.TextComponent.text = stat.Current.ToString();
                break;
            case EnumStatValue.Minimum:
                _statisticHandler.TextComponent.text = stat.Minimum.ToString();
                break;
            case EnumStatValue.Maximum:
                _statisticHandler.TextComponent.text = stat.Maximum.ToString();
                break;
            case EnumStatValue.Percent:
                _statisticHandler.TextComponent.text = stat.GetPercent().ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
