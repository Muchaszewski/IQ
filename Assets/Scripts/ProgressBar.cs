using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    public Image ProgressBarImage;

    public Color BackgroundColor = Color.gray;
    public Color BarColor = Color.red;

    public EnumTypeStat ValueName;
    public EnumCurrentEntity CurrentEntityType;

    private StatValueFloat statValueFloat;

    public enum EnumCurrentEntity
    {
        Player,
        Enemy,
    }

    // Use this for initialization
    void Start()
    {
        UpdateTarget();
        GetComponent<Image>().color = BackgroundColor;
        ProgressBarImage.color = BarColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (statValueFloat != null)
        {
            ProgressBarImage.transform.localScale = new Vector3(statValueFloat.GetPercent() / 100f, 1, 1);
        }
        else
        {
            ProgressBarImage.transform.localScale = new Vector3(0, 1, 1);
        }
    }

    /// <summary>
    /// Update reference to StatValueFloat
    /// </summary>
    void UpdateTarget()
    {
        var fightController = CurrentGame.Instance.FightController;

        switch (CurrentEntityType)
        {
            case EnumCurrentEntity.Player:
                statValueFloat =
                    (StatValueFloat)fightController.Player.Stats.GetType()
                        .GetProperty(ValueName.ToString())
                        .GetValue(fightController.Player.Stats, null);
                break;
            case EnumCurrentEntity.Enemy:
                try
                {
                    statValueFloat =
                        (StatValueFloat)fightController.Target.Stats.GetType()
                            .GetProperty(ValueName.ToString())
                            .GetValue(fightController.Target.Stats, null);
                }
                catch
                {
                    Debug.LogError("Target does not exists");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
