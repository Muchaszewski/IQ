using System;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;
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

    private Text _textComponent;
    private object _statReference;

    void Start()
    {
        var _player = CurrentGame.Instance.Player;
        _textComponent = GetComponent<Text>();
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
                _statReference = reflectedField.GetValue(reflectedValue, null);
                break;
            case EnumStatisticHandler.Skill:
                if (level)
                {
                    _statReference = _player.PasiveSkills.GetSkillLevelByEnum(skill);
                }
                else
                {
                    _statReference = _player.PasiveSkills.GetSkillExperienceByEnum(skill);
                }
                break;
            case EnumStatisticHandler.Entity:
                switch (entityStatType)
                {
                    case EnumPlayerBasics.Name:
                        _statReference = _player.Name;
                        break;
                    case EnumPlayerBasics.Sex:
                        _statReference = _player.Sex;
                        break;
                    case EnumPlayerBasics.Type:
                        _statReference = _player.Type;
                        break;
                    case EnumPlayerBasics.Level:
                        _statReference = _player.Level;
                        break;
                    case EnumPlayerBasics.Experience:
                        _statReference = _player.Experience;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Update()
    {
        _textComponent.text = _statReference.ToString();
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
