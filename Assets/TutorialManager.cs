using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InventoryQuest.Game;

public class TutorialManager : MonoBehaviour
{

    public CharacterCreationHead CharacterCreationHead;
    public Image PlayerHead;
    public GameObject GroupCombat;
    public GameObject GroupHeadCreation;

    public InputField InputNameField;
    public Text PlayerNameText;

    public GameObject HeadAndNameApplyButton;



    public GameObject GroupCharacterCreation;
    public GameObject GroupStatistics;
    public Sprite StatiscicsPanel;
    public Text TotalStatsValue;

    public int StartPlayerStats = 6;
    public int MinPlayerStats = 6;
    public int MaxPlayerStats = 18;

    private int[] rolledStats = new int[] { 0, 0, 0, 0, 0, 0 };
    private int[] savedStats = new int[] { 0, 0, 0, 0, 0, 0 };


    void Start()
    {
        // Initial player's stats
        ApplyStats(new int[] { StartPlayerStats, StartPlayerStats, StartPlayerStats, StartPlayerStats, StartPlayerStats, StartPlayerStats });

        // Initial stat roll
        RollStats();
    }

    public void ChooseHead()
    {
        if (InputNameField.text.Length > 0)
        {
            CurrentGame.Instance.Player.Name = InputNameField.text;
            PlayerNameText.text = CurrentGame.Instance.Player.Name;

            PlayerHead.sprite = CharacterCreationHead.CurrentHead;
            GroupCombat.SetActive(true);
            GroupHeadCreation.SetActive(false);
        }
    }

    /// <summary>
    /// Update player's with values from statsTpApply.
    /// </summary>
    /// <param name="statsToApply"></param>
    private void ApplyStats (int[] statsToApply)
    {
        var stats = CurrentGame.Instance.Player.Stats;

        stats.Strength.Current      = stats.Strength.Base       = statsToApply[0];
        stats.Dexterity.Current     = stats.Dexterity.Base      = statsToApply[1];
        stats.Perception.Current    = stats.Perception.Base     = statsToApply[2];
        stats.Wisdom.Current        = stats.Wisdom.Base         = statsToApply[3];
        stats.Intelligence.Current  = stats.Intelligence.Base   = statsToApply[4];
        stats.Vitality.Current      = stats.Vitality.Base       = statsToApply[5];
    }

    /// <summary>
    /// Returns the sum of all the rolled stats
    /// </summary>
    /// <returns></returns>
    public int CalculateRolledStatsTotal()
    {
        int statsSum = 0;
        for (int i = 0; i<=5 ; i++)
        {
            statsSum += rolledStats[i];
        }
        return statsSum;
    }

    /// <summary>
    /// Generate random stats and calculate their total.
    /// </summary>
    public void RollStats()
    {
        // Roll the stats
        var diff = MaxPlayerStats - MinPlayerStats;
        for (int i = 0; i<=5 ; i++)
        {
            rolledStats[i] = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        }
        
        // Display the stats
        refreshStatsDisplay();
        // Display the total
        TotalStatsValue.text = CalculateRolledStatsTotal().ToString();
    }

    /// <summary>
    /// Temporarily save the rolled stats.
    /// </summary>
    public void SaveStats()
    {
        savedStats[0] = rolledStats[0];
        savedStats[1] = rolledStats[1];
        savedStats[2] = rolledStats[2];
        savedStats[3] = rolledStats[3];
        savedStats[4] = rolledStats[4];
        savedStats[5] = rolledStats[5];
    }

    /// <summary>
    /// Load the stat roll saved with the SaveStats() method.
    /// </summary>
    public void LoadStats()
    {
        rolledStats[0] = savedStats[0];
        rolledStats[1] = savedStats[1];
        rolledStats[2] = savedStats[2];
        rolledStats[3] = savedStats[3];
        rolledStats[4] = savedStats[4];
        rolledStats[5] = savedStats[5];

        // Display the stats
        refreshStatsDisplay();
        // Display the total
        TotalStatsValue.text = CalculateRolledStatsTotal().ToString();
    }

    /// <summary>
    /// Temporary method that should only update the display.
    /// The stats should be applied only after the player accepts them, and not after every roll.
    /// The display is handled somewhere else, hence this method justs updates the stat values.
    /// </summary>
    private void refreshStatsDisplay()
    {
        ApplyStats(rolledStats);
    }

    public void ChangedName()
    {
        // Only allow apllying the portrait and the name if the name is not empty
        if (InputNameField.text.Length > 0)
        {
            Button buttonComponent = HeadAndNameApplyButton.GetComponent<Button>();
            buttonComponent.interactable = true;
        }
        else
        {
            Button buttonComponent = HeadAndNameApplyButton.GetComponent<Button>();
            buttonComponent.interactable = false;
        }
    }

    public void ApplyCharacter()
    {
        var stats = CurrentGame.Instance.Player.Stats;

        ApplyStats(rolledStats);

        GroupStatistics.transform.parent.GetComponent<Image>().sprite = StatiscicsPanel;
        GroupStatistics.SetActive(true);
        GroupCharacterCreation.SetActive(false);
    }

}
