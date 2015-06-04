using System;
using UnityEngine;
using System.Collections;
using InventoryQuest;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GetStat : MonoBehaviour
{

    [Tooltip("Stats object")]
    public EnumStatObject objectStats;

    [Tooltip("Type of displayed stat")]
    public EnumItemClass itemType;

    [Tooltip("Type of entite stat to display")]
    public EnumTypeStat typeStat;

    [Tooltip("Type of displayed stat")]
    public EnumStatValue valueType = EnumStatValue.Name;

    [Tooltip("Only applays to field if Custom Type is choosen")]
    public string customString;

    [Tooltip("Is this stat responsible for changeing Text component if requireds are met")]
    public bool isDisplayed = true;

    public Stats Stats { get; set; }

    private Text _textComponent;

    // Use this for initialization
    void Start()
    {
        _textComponent = GetComponent<Text>();
        _textComponent.text = SetDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

#if UNITY_EDITOR
    public string SetPreview()
    {
        switch (valueType)
        {
            case EnumStatValue.ShortName:
                var name = typeStat.GetAttributeOfType<NameAttribute>();
                return name.ShortName;
                break;
            case EnumStatValue.Name:
                return typeStat.GetAttributeOfType<NameAttribute>().LongName;
                break;
            case EnumStatValue.Extend:
                return 999.ToString();
                break;
            case EnumStatValue.Base:
                return 100.ToString();
                break;
            case EnumStatValue.Current:
                return 10.ToString();
                break;
            case EnumStatValue.Minimum:
                return 0.ToString();
                break;
            case EnumStatValue.Maximum:
                return int.MaxValue.ToString();
                break;
            case EnumStatValue.Percent:
                return 35.ToString();
                break;
            case EnumStatValue.Scale:
                return typeStat.GetAttributeOfType<StatScaleAttribute>().Scale.ToString();
                break;
            case EnumStatValue.Type:
                return "Not implemented yet";
                break;
            case EnumStatValue.Custom:
                return customString;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return null;
    }
#endif

    public string SetDisplay()
    {
        switch (valueType)
        {
            case EnumStatValue.ShortName:
                var name = typeStat.GetAttributeOfType<NameAttribute>();
                return name.ShortName;
                break;
            case EnumStatValue.Name:
                return typeStat.GetAttributeOfType<NameAttribute>().LongName;
                break;
            case EnumStatValue.Extend:
                return 999.ToString();
                break;
            case EnumStatValue.Base:
                return 100.ToString();
                break;
            case EnumStatValue.Current:
                return 10.ToString();
                break;
            case EnumStatValue.Minimum:
                return 0.ToString();
                break;
            case EnumStatValue.Maximum:
                return int.MaxValue.ToString();
                break;
            case EnumStatValue.Percent:
                return 35.ToString();
                break;
            case EnumStatValue.Scale:
                return typeStat.GetAttributeOfType<StatScaleAttribute>().Scale.ToString();
                break;
            case EnumStatValue.Type:
                return "Not implemented yet";
                break;
            case EnumStatValue.Custom:
                return customString;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return null;
    }

    public enum EnumStatObject
    {
        CurrentPlayer,
        SelectedItem,
        SelectedEnemy,
    }

    public enum EnumItemClass
    {
        Weapon,
        Armor,
    }
}