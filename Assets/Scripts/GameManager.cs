using UnityEngine;
using System.Collections;
using InventoryQuest.Game;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        CurrentGame.Save();
    }

    public void Load()
    {
       var player =  CurrentGame.Load();
    }
}
