using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour
{
    void Start()
    {
        FightController.onAttack += FightController_onAttack;
    }

    public void OnDestroy()
    {
        FightController.onAttack -= FightController_onAttack;
    }

    private void FightController_onAttack(object sender, FightControllerEventArgs e)
    {
        if (e.Target.Equals(CurrentGame.Instance.Player))
        {
            GetComponent<FloatingText>().CreateFloatingText(e.Message);
        }
    }
}
