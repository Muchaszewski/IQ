using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using InventoryQuest.InventoryQuest.Components.ActionEvents;
using UnityEngine.UI;

public class SpecialTravelingText : MonoBehaviour
{

    public string RefilingString = "Refilling";
    public string TravellingString = "Traveling";
    public string LookingForEnemiesString = "Looking For Enemies";
    public string WillTravelString = "Will Travel";
    public string Other = "";

    private Text _text;
    private int _shouldReset;

    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = Other;

        ActionEventManager.Fight.OnTravelBegin += CurrentGame_TravelingBegin;
        ActionEventManager.Fight.OnTravelEnd += CurrentGame_TravelingFinished;
        ActionEventManager.Regen.StaminaRegen.OnBegin += CurrentGame_RefilingBegin;
        ActionEventManager.Regen.StaminaRegen.OnEnd += CurrentGame_RefilingFinieshed;
        ActionEventManager.Fight.OnLookingForEnemies += CurrentGame_LookingForEnemies;
        ActionEventManager.Fight.OnEnemiesFound += CurrentGame_EnemiesFound;
        ActionEventManager.Fight.OnTravelPlanned += CurrentGame_WillChangeSpot;
    }

    private void CurrentGame_WillChangeSpot(object sender, System.EventArgs e)
    {
        _text.text = WillTravelString;
    }

    private void CurrentGame_EnemiesFound(object sender, System.EventArgs e)
    {
        if (_shouldReset == 2)
        {
            _text.text = Other;
        }
    }

    private void CurrentGame_LookingForEnemies(object sender, System.EventArgs e)
    {
        _shouldReset = 2;
        _text.text = LookingForEnemiesString;
    }

    private void CurrentGame_RefilingFinieshed(object sender, System.EventArgs e)
    {
        if (_shouldReset == 1)
        {
            _text.text = Other;
        }
    }

    private void CurrentGame_RefilingBegin(object sender, System.EventArgs e)
    {
        _shouldReset = 1;
        _text.text = RefilingString;
    }

    private void CurrentGame_TravelingFinished(object sender, System.EventArgs e)
    {
        if (_shouldReset == 3)
        {
            _text.text = Other;
        }
    }

    private void CurrentGame_TravelingBegin(object sender, System.EventArgs e)
    {
        _shouldReset = 3;
        _text.text = TravellingString;
    }
}
