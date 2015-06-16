using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using UnityEngine;

#if UNITY_EDITOR

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

#endif