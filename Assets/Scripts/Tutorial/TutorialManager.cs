﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InventoryQuest.Game;
using System.Linq;
using Extensions;

// ReSharper disable All

public class TutorialManager : MonoBehaviour
{

    public CharacterCreationHead CharacterCreationHead;
    public Image PlayerHead;
    public GameObject GroupCombat;
    public GameObject GroupHeadCreation;

    public InputField InputNameField;
    public Text PlayerNameText;
    public Text PlayerNameText2;

    public GameObject HeadAndNameApplyButton;

    public GameObject GroupCharacterCreation;
    public GameObject GroupStatistics;
    public Sprite StatiscicsPanel;
    public Text TotalStatsValue;

    public int StartPlayerStats = 6;
    public int MinPlayerStats = 6;
    public int MaxPlayerStats = 18;

    private int[] _rolledStats = { 0, 0, 0, 0, 0, 0 };
    private int[] _savedStats = { 6, 6, 6, 6, 6, 6 };

    void Start()
    {
        // Initial player's stats
        ApplyStats(new int[] { StartPlayerStats, StartPlayerStats, StartPlayerStats, StartPlayerStats, StartPlayerStats, StartPlayerStats });
        CurrentGame.Instance.Player.SetAllBaseStats();
        // Initial stat roll
        RollStats();

        GroupStatistics.SetActive(false);
        GroupCharacterCreation.SetActive(true);
        GroupCombat.SetActive(false);
        GroupHeadCreation.SetActive(true);

#if UNITY_EDITOR
        InputNameField.text = "TEST";
#endif
    }

    public void ChooseHead()
    {
        if (InputNameField.text.Length > 0)
        {
            CurrentGame.Instance.Player.Name = InputNameField.text;
            PlayerNameText.text = CurrentGame.Instance.Player.Name;
            PlayerNameText2.text = CurrentGame.Instance.Player.Name;

            PlayerHead.sprite = CharacterCreationHead.CurrentHead;
            GroupCombat.SetActive(true);
            GroupHeadCreation.SetActive(false);
        }
        TutorialActions.Instance.TutorialIntroduction();
    }

    /// <summary>
    /// Update player's with values from statsTpApply.
    /// </summary>
    /// <param name="statsToApply"></param>
    private void ApplyStats(int[] statsToApply)
    {
        var stats = CurrentGame.Instance.Player.Stats;

        stats.Strength.Current = stats.Strength.Base = statsToApply[0];
        stats.Dexterity.Current = stats.Dexterity.Base = statsToApply[1];
        stats.Perception.Current = stats.Perception.Base = statsToApply[2];
        stats.Wisdom.Current = stats.Wisdom.Base = statsToApply[3];
        stats.Intelligence.Current = stats.Intelligence.Base = statsToApply[4];
        stats.Vitality.Current = stats.Vitality.Base = statsToApply[5];

        CurrentGame.Instance.Player.SetAllBaseStats();
    }

    /// <summary>
    /// Generate random stats and calculate their total.
    /// </summary>
    public void RollStats()
    {
        // Roll the stats
        var diff = MaxPlayerStats - MinPlayerStats;
        for (int i = 0; i <= 5; i++)
        {
            _rolledStats[i] = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        }

        // Display the stats
        ApplyStats(_rolledStats);
        // Display the total
        TotalStatsValue.text = _rolledStats.Sum().ToString();
    }

    /// <summary>
    /// Temporarily save the rolled stats.
    /// </summary>
    public void SaveStats()
    {
        _rolledStats.CopyTo(_savedStats, 0);
    }

    /// <summary>
    /// Load the stat roll saved with the SaveStats() method.
    /// </summary>
    public void LoadStats()
    {
        _savedStats.CopyTo(_rolledStats, 0);

        // Display the stats
        ApplyStats(_rolledStats);
        // Display the total
        TotalStatsValue.text = _rolledStats.Sum().ToString();
    }

    public void ChangedName()
    {
        // Only allow apllying the portrait and the name if the name is not empty
        if (InputNameField.text.Length > 0)
        {
            ExtendedButton buttonComponent = HeadAndNameApplyButton.GetComponent<ExtendedButton>();
            buttonComponent.interactable = true;
        }
        else
        {
            ExtendedButton buttonComponent = HeadAndNameApplyButton.GetComponent<ExtendedButton>();
            buttonComponent.interactable = false;
        }
    }

    public void ApplyCharacter()
    {
        ApplyStats(_rolledStats);

        GroupStatistics.transform.parent.GetComponent<Image>().sprite = StatiscicsPanel;
        GroupStatistics.SetActive(true);
        GroupCharacterCreation.SetActive(false);
        TutorialActions.Instance.TutorialEquipmentOpen();

    }

}
