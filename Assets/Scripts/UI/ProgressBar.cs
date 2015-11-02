using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public int Special;

    private List<Action> _actions = new List<Action>();

    private StatValueFloat statValueFloat;

    public enum EnumCurrentEntity
    {
        Player,
        Enemy,
        Special,
    }

    // Use this for initialization
    void Start()
    {
        UpdateTarget();
        GetComponent<Image>().color = BackgroundColor;
        ProgressBarImage.color = BarColor;
        SetActions();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEntityType == EnumCurrentEntity.Special)
        {
            _actions[Special].Invoke();
            return;
        }
        if (CurrentEntityType == EnumCurrentEntity.Enemy)
        {
            try
            {
                statValueFloat = CurrentGame.Instance.FightController.Target.Stats.HealthPoints;
            }
            catch { }
        }
        if (statValueFloat != null)
        {
            if (statValueFloat.Current <= 0)
            {
                ProgressBarImage.transform.localScale = new Vector3(0, 1, 1);
            }
            else
            {
                ProgressBarImage.transform.localScale = new Vector3(statValueFloat.GetPercent() / 100f, 1, 1);
            }
        }
        else
        {
            UpdateTarget();
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

                break;
            case EnumCurrentEntity.Special:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void SetActions()
    {
        _actions.Add(null);
        _actions[0] = (delegate
        {
            //Experience
            ProgressBarImage.transform.localScale =
                new Vector3((float)CurrentGame.Instance.Player.Experience /
                (float)CurrentGame.Instance.Player.GetToNextLevelExperience(), 1, 1);
        });

        _actions.Add(null);
        _actions[1] = (delegate
        {
            //AreaProgress
            if (CurrentGame.Instance.Spot.MonsterValueToCompleteArea == 0)
            {
                ProgressBarImage.transform.localScale = new Vector3(1, 1, 1);
                return;
            }
            ProgressBarImage.transform.localScale =
                new Vector3((float)CurrentGame.Instance.AreaProgress /
                (float)CurrentGame.Instance.Spot.MonsterValueToCompleteArea, 1, 1);
        });
    }

}
