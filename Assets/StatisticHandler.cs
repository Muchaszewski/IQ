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

    public Text TextComponent { get; private set; }
    public object StatReference { get; private set; }

    void Start()
    {
        var _player = CurrentGame.Instance.Player;
        TextComponent = GetComponent<Text>();
        switch (statType)
        {
            case EnumStatisticHandler.Special:

                break;
            case EnumStatisticHandler.Stat:
                var reflectedType = _player.Stats.GetType();
                var reflectedField = reflectedType.GetProperty(stat.ToString());
                var reflectedValue = reflectedField.GetValue(_player.Stats, null);
                reflectedType = reflectedValue.GetType();
                reflectedField = reflectedType.GetProperty(value.ToString());
                StatReference = reflectedField.GetValue(reflectedValue, null);
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
                        break;
                    case EnumPlayerBasics.Experience:
                        StatReference = _player.Experience;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        TextComponent.text = StatReference.ToString();
    }
}

public enum EnumStatisticHandler
{
    Special,
    Stat,
    Skill,
    Entity,
}

public enum EnumPlayerBasics
{
    Name,
    Sex,
    Type,
    Level,
    Experience
}
