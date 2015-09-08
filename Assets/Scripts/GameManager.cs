using UnityEngine;
using System.Collections;
using InventoryQuest.Game;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public Color StatisticsBuffColor = Color.blue;
    public Color StatisticsDebuffColor = Color.red;
    public Color StatisciscNormalColor = Color.white;

}
