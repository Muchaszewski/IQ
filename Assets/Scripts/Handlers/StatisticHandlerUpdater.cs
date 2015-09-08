using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;

[RequireComponent(typeof(StatisticHandler))]
[DisallowMultipleComponent]
public class StatisticHandlerUpdater : MonoBehaviour
{
    public string OutputFormat = "####";

    private StatisticHandler _statisticHandler;

    private StatValueInt _statValueInt;
    private StatValueFloat _statValueFloat;


    void Start()
    {
        _statisticHandler = GetComponent<StatisticHandler>();
        if (_statisticHandler.StatReference != null)
        {
            if (_statisticHandler.StatReference.GetType() == typeof(StatValueFloat))
            {
                _statValueFloat = (_statisticHandler.StatReference as StatValueFloat);
            }
            else if (_statisticHandler.StatReference.GetType() == typeof(StatValueInt))
            {
                _statValueInt = (_statisticHandler.StatReference as StatValueInt);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_statisticHandler.statType)
        {
            case EnumStatisticHandler.Stat:
                if (_statisticHandler.stat == EnumTypeStat.DPS)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.DPS.ToString(OutputFormat);
                    break;
                }
                if (_statisticHandler.stat == EnumTypeStat.Accuracy)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.Accuracy.ToString(OutputFormat);
                    break;
                }
                if (_statisticHandler.stat == EnumTypeStat.AttackSpeed)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.AttackSpeed.ToString(OutputFormat);
                    break;
                }
                if (_statisticHandler.stat == EnumTypeStat.Deflection)
                {
                    _statisticHandler.TextComponent.text = CurrentGame.Instance.Player.Parry.ToString(OutputFormat);
                    break;
                }
                if (_statValueFloat != null)
                {
                    SetStatText(_statValueFloat);
                }
                else if (_statValueInt != null)
                {
                    SetStatText(_statValueInt);
                }
                break;
            case EnumStatisticHandler.Target:
                _statisticHandler.TextComponent.text = CurrentGame.Instance.FightController.Target.Name;
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
                _statisticHandler.TextComponent.text = Convert.ToSingle(_statisticHandler.StatReference).ToString(OutputFormat);
                break;
        }

        if (_statValueFloat != null)
        {
            if (_statValueFloat.Extend > _statValueFloat.Base)
            {
                _statisticHandler.TextComponent.color = GameManager.Instance.StatisticsBuffColor;
            }
            else if (_statValueFloat.Extend < _statValueFloat.Base)
            {
                _statisticHandler.TextComponent.color = GameManager.Instance.StatisticsDebuffColor;
            }
        }
        else if (_statValueInt != null)
        {
            if (_statValueInt.Extend > _statValueInt.Base)
            {
                _statisticHandler.TextComponent.color = GameManager.Instance.StatisticsBuffColor;
            }
            else if (_statValueInt.Extend < _statValueInt.Base)
            {
                _statisticHandler.TextComponent.color = GameManager.Instance.StatisticsDebuffColor;
            }
        }
    }

    void SetStatText<T>(IStatValue<T> stat)
    {
        switch (_statisticHandler.value)
        {
            case EnumStatValue.Extend:
                _statisticHandler.TextComponent.text = Convert.ToSingle(stat.Extend).ToString(OutputFormat);
                break;
            case EnumStatValue.Base:
                _statisticHandler.TextComponent.text = Convert.ToSingle(stat.Base).ToString(OutputFormat);
                break;
            case EnumStatValue.Current:
                _statisticHandler.TextComponent.text = Convert.ToSingle(stat.Current).ToString(OutputFormat);
                break;
            case EnumStatValue.Minimum:
                _statisticHandler.TextComponent.text = Convert.ToSingle(stat.Minimum).ToString(OutputFormat);
                break;
            case EnumStatValue.Maximum:
                _statisticHandler.TextComponent.text = Convert.ToSingle(stat.Maximum).ToString(OutputFormat);
                break;
            case EnumStatValue.Percent:
                _statisticHandler.TextComponent.text = stat.GetPercent().ToString(OutputFormat);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
    