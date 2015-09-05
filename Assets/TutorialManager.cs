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

    public int StartPlayerStats = 8;
    public int MinPlayerStats = 6;
    public int MaxPlayerStats = 18;

    public InputField InputNameField;
    public Text PlayerNameText;

    public GameObject GroupCharacterCreation;
    public GameObject GroupStatistics;
    public Sprite StatiscicsPanel;

    void Start()
    {

    }

    public void ChooseHead()
    {
        PlayerHead.sprite = CharacterCreationHead.CurrentHead;
        GroupCombat.SetActive(true);
        GroupHeadCreation.SetActive(false);
    }

    public void RollStats()
    {
        var diff = MaxPlayerStats - MinPlayerStats;
        var stats = CurrentGame.Instance.Player.Stats;
        stats.Strength.Current = stats.Strength.Base = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        stats.Dexterity.Current = stats.Dexterity.Base = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        stats.Perception.Current = stats.Perception.Base = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        stats.Wisdom.Current = stats.Wisdom.Base = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        stats.Intelligence.Current = stats.Intelligence.Base = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
        stats.Vitality.Current = stats.Vitality.Base = Mathf.CeilToInt(TransformUtils.RandomNormalizedGaussian() * diff) + MinPlayerStats;
    }

    public void ChangedName()
    {
        CurrentGame.Instance.Player.Name = InputNameField.text;
        PlayerNameText.text = CurrentGame.Instance.Player.Name;
    }

    public void ApplyCharacter()
    {
        GroupStatistics.transform.parent.GetComponent<Image>().sprite = StatiscicsPanel;
        GroupStatistics.SetActive(true);
        GroupCharacterCreation.SetActive(false);
    }

}
