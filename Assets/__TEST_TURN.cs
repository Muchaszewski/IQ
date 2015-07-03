using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InventoryQuest.Game;

public class __TEST_TURN : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = CurrentGame.Instance.FightController.Player.NextTurn + " " +
                                    CurrentGame.Instance.FightController.Enemy[0].NextTurn +
                                    " " + CurrentGame.Instance.FightController.Player.Stats.HealthPoints +
                                    " " + CurrentGame.Instance.FightController.Player.Stats.Armor +
                                    " " + CurrentGame.Instance.FightController.Player.Stats.Armor.Extend
                                    + "/r/n" + CurrentGame.Instance.FightController.Enemy[0].Stats.StaminaPoints.Current;
    }
}
