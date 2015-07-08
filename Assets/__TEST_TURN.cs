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
        GetComponent<Text>().text =
                       "Player Turn: "  + CurrentGame.Instance.FightController.Player.NextTurn
            + "\r\n" + "Next Turn: "    + CurrentGame.Instance.FightController.Enemy[0].NextTurn
            + "\r\n" + "Health: "       + CurrentGame.Instance.FightController.Player.Stats.HealthPoints
            + "\r\n" + "Armor: "        + CurrentGame.Instance.FightController.Player.Stats.Armor + " / " + CurrentGame.Instance.FightController.Player.Stats.Armor.Extend
            + "\r\n" + "Stamina: "      + CurrentGame.Instance.FightController.Enemy[0].Stats.StaminaPoints.Current;
    }
}
