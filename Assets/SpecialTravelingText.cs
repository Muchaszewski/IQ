using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using UnityEngine.UI;

public class SpecialTravelingText : MonoBehaviour
{

    public string RefilingString = "Refilling";
    public string TravellingString = "Traveling";
    public string LookingForEnemiesString = "Looking For Enemies";
    public string WillTravelString = "Will Travel";

    private Text _text;
    private int _shouldReset;

    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = "";

        CurrentGame.TravelingBegin += CurrentGame_TravelingBegin;
        CurrentGame.TravelingFinished += CurrentGame_TravelingFinished;
        CurrentGame.RefilingBegin += CurrentGame_RefilingBegin;
        CurrentGame.RefilingFinieshed += CurrentGame_RefilingFinieshed;
        CurrentGame.LookingForEnemies += CurrentGame_LookingForEnemies;
        CurrentGame.EnemiesFound += CurrentGame_EnemiesFound;
        CurrentGame.WillChangeSpot += CurrentGame_WillChangeSpot;
    }

    private void CurrentGame_WillChangeSpot(object sender, System.EventArgs e)
    {
        _text.text = WillTravelString;
    }

    private void CurrentGame_EnemiesFound(object sender, System.EventArgs e)
    {
        if (_shouldReset == 2)
        {
            _text.text = "";
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
            _text.text = "";
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
            _text.text = "";
        }
    }

    private void CurrentGame_TravelingBegin(object sender, System.EventArgs e)
    {
        _shouldReset = 3;
        _text.text = TravellingString;
    }
}
