using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Entities.Money;
using InventoryQuest.Components.Entities.Player;
using InventoryQuest.Game;
using UnityEngine.UI;

public class UIMoney : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private GameObject[] Coins;
    [HideInInspector]
    [SerializeField]
    private Text[] Value;

    private Player _player;

    void Start()
    {
        _player = CurrentGame.Instance.Player;
        _player.Wallet.AddedMoney += Wallet_AddedMoney;
        Wallet_AddedMoney(this, EventArgs.Empty);
    }

    private void Wallet_AddedMoney(object sender, System.EventArgs e)
    {
        var wallet = _player.Wallet;
        Value[0].text = (wallet.GetCurrentAmount() % 100).ToString();
        if (wallet.GetCurrentAmount(Currency.Silver) == 0)
        {
            Coins[1].SetActive(false);
        }
        else
        {
            Coins[1].SetActive(true);
            Value[1].text = (wallet.GetCurrentAmount(Currency.Silver) % 100).ToString();
        }
        if (wallet.GetCurrentAmount(Currency.Gold) == 0)
        {
            Coins[2].SetActive(false);
        }
        else
        {
            Coins[2].SetActive(true);
            Value[2].text = (wallet.GetCurrentAmount(Currency.Gold)).ToString();
        }
        //if (wallet.GetCurrentAmount(Currency.Platinum) == 0)
        //{
        //    Coins[3].SetActive(false);
        //}
        //else
        //{
        //    Coins[3].SetActive(true);
        //    Value[3].text = wallet.GetCurrentAmount(Currency.Platinum).ToString();
        //}
    }
}
