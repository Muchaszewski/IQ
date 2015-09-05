using System;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class StatisticHandler : MonoBehaviour
{
    public EnumStatisticHandler statType = EnumStatisticHandler.Stat;
    [HideInInspector]
    public bool level;
    [HideInInspector]
    public EnumItemClassSkill skill;
    [HideInInspector]
    public EnumTypeStat stat;
    [HideInInspector]
    public EnumStatValue value;
    [HideInInspector]
    public EnumPlayerBasics entityStatType;
    [HideInInspector]
    public string statName;

    public Text TextComponent { get; private set; }
    public object StatReference { get; set; }

    void Start()
    {
        var _player = CurrentGame.Instance.Player;
        TextComponent = GetComponent<Text>();
        switch (statType)
        {
            case EnumStatisticHandler.Stat:
                if (stat == EnumTypeStat.DPS)
                {
                    StatReference = _player.DPS;
                    break;
                }
                if (stat == EnumTypeStat.Accuracy)
                {
                    StatReference = _player.Accuracy;
                    break;
                }
                if (stat == EnumTypeStat.AttackSpeed)
                {
                    StatReference = _player.AttackSpeed;
                    break;
                }
                if (stat == EnumTypeStat.Deflection)
                {
                    StatReference = _player.Parry;
                    break;
                }
                var reflectedType = _player.Stats.GetType();
                var reflectedField = reflectedType.GetProperty(stat.ToString());
                var reflectedValue = reflectedField.GetValue(_player.Stats, null);
                if (reflectedValue.GetType() == typeof(StatValueFloat))
                {
                    StatReference = _player.Stats.GetStatFloatByEnum(stat);
                }
                else if (reflectedValue.GetType() == typeof(StatValueInt))
                {
                    StatReference = _player.Stats.GetStatIntByEnum(stat);
                }

                reflectedType = reflectedValue.GetType();
                reflectedField = reflectedType.GetProperty(value.ToString());
                TextComponent.text = reflectedField.GetValue(reflectedValue, null).ToString();

                break;
            case EnumStatisticHandler.Skill:
                if (level)
                {
                    StatReference = _player.PasiveSkills.GetSkillLevelByEnum(skill);
                }
                else
                {
                    StatReference = _player.PasiveSkills.GetSkillExperienceByEnum(skill);
                }
                break;
            case EnumStatisticHandler.Entity:
                switch (entityStatType)
                {
                    case EnumPlayerBasics.Name:
                        StatReference = _player.Name;
                        break;
                    case EnumPlayerBasics.Sex:
                        StatReference = _player.Sex;
                        break;
                    case EnumPlayerBasics.Type:
                        StatReference = _player.Type;
                        break;
                    case EnumPlayerBasics.Level:
                        StatReference = _player.Level;
                        TextComponent.text = StatReference != null ? "Level " + StatReference.ToString() : "";
                        return;
                    case EnumPlayerBasics.Experience:
                        StatReference = _player.Experience;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case EnumStatisticHandler.Target:
                //LOGIC IN UPDATE
                break;
            case EnumStatisticHandler.Special:
                ApplySpecial();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        TextComponent.text = StatReference != null ? StatReference.ToString() : "";
    }

    public void RecalculateStatistics()
    {
        Start();
    }

    public void ApplySpecial()
    {
        var _player = CurrentGame.Instance.Player;
        if (statName == "DPS")
        {
            TextComponent.text = _player.MinDamage + "-" + _player.MaxDamage;
            return;
        }
    }
}

public enum EnumStatisticHandler
{
    Special,
    Stat,
    Skill,
    Entity,
    Target,
}

public enum EnumPlayerBasics
{
    Name,
    Sex,
    Type,
    Level,
    Experience
}
