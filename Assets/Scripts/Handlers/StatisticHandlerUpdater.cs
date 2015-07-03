using System;
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
                if (_statisticHandler.StatReference.GetType() == typeof(StatValueFloat))
                {
                    SetStatText(_statisticHandler.StatReference as StatValueFloat);
                }
                else if (_statisticHandler.StatReference.GetType() == typeof(StatValueInt))
                {
                    SetStatText(_statisticHandler.StatReference as StatValueInt);
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
