using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventoryQuest.Components.Entities.Player.Inventory;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;
using UnityEngine.UI;

public class TutorialActions : MonoBehaviour
{
    public static TutorialActions Instance { get; set; }

    public MessageBox MessageBox;

    public Button[] MenuButtons;
    public Button ApplyStatisticsButton;

    private int _attackCount = 0;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        for (int i = 0; i < MenuButtons.Length - 1; i++)
        {
            MenuButtons[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {

    }

    //TODO Change methods name for proper actions @MaciejLitwin
    public void TutorialIntroduction()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(-200, 214);
        MessageBox.TitleText.text = "おはよ！";
        MessageBox.MessageText.text = "よおこそ！自転車さま。げんきですか？この世界すばらしですね？";
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(TutorialAutoKill);
    }

    public void TutorialAutoKill()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(200, 214);
        MessageBox.TitleText.text = "モンスタ！";
        MessageBox.MessageText.text = "あなたはたたかうモンスタ自動的に、1戦いに勝ちます";
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.gameObject.SetActive(false);
        FightController.onVicotry += FightController_onVicotry;
        MessageBox.NextButton.onClick.AddListener(TutorialKillMonsters);
    }

    private void FightController_onVicotry(object sender, FightControllerEventArgs e)
    {
        MessageBox.NextButton.gameObject.SetActive(true);
        FightController.onVicotry -= FightController_onVicotry;
    }

    public void TutorialKillMonsters()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(10, 214);
        MessageBox.TitleText.text = "モンスタ";
        MessageBox.MessageText.text = "攻撃25回！";
        MessageBox.NextButton.gameObject.SetActive(false);
        FightController.onAttack += FightController_onAttack;
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(TutorialOpenStats);
    }

    private void FightController_onAttack(object sender, FightControllerEventArgs e)
    {
        _attackCount++;
        if (_attackCount == 25 && e.Invoker == e.FightController.Player)
        {
            MessageBox.NextButton.gameObject.SetActive(true);
            FightController.onAttack -= FightController_onAttack;
        }
    }

    private void TutorialOpenStats()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(10, 214);
        MessageBox.TitleText.text = "統計";
        MessageBox.MessageText.text = "オープン統計";
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(TutorialAutoKill);
        MessageBox.NextButton.gameObject.SetActive(false);
        MenuButtons[0].gameObject.SetActive(true);
        MenuButtons[0].onClick.AddListener(TutorialSetCharacter);
    }

    private void TutorialSetCharacter()
    {
        MenuButtons[0].onClick.RemoveListener(TutorialSetCharacter);
        MessageBox.RectTransform.anchoredPosition = new Vector2(-617, -314);
        MessageBox.TitleText.text = "統計";
        MessageBox.MessageText.text = "統計を取得します";
        MessageBox.NextButton.gameObject.SetActive(false);
    }

    public void TutorialEquipmentOpen()
    {
        MenuButtons[4].gameObject.SetActive(true);
        MenuButtons[4].onClick.AddListener(TutorialEquipItems);
        MessageBox.RectTransform.anchoredPosition = new Vector2(10, 214);
        MessageBox.TitleText.text = "機器";
        MessageBox.MessageText.text = "オープン施設";
        MessageBox.NextButton.gameObject.SetActive(false);
    }


    private void TutorialEquipItems()
    {
        MenuButtons[4].onClick.RemoveListener(TutorialEquipItems);
        CurrentGame.Instance.Player.Inventory.Items = new SortedList<int, Item>();
        InventoryPanel.Instance.PopulateInventory();
        MessageBox.RectTransform.anchoredPosition = new Vector2(-630, 230);
        MessageBox.TitleText.text = "機器";
        MessageBox.MessageText.text = "3項目ドレス";
        MessageBox.NextButton.gameObject.SetActive(false);
        Inventory.EventItemSwaped += Inventory_EventItemSwaped;
    }

    private void Inventory_EventItemSwaped(object sender, System.EventArgs e)
    {
        var count = 0;
        foreach (var item in CurrentGame.Instance.Player.Inventory.Items)
        {
            if (item.Key < 0)
            {
                count++;
            }
            else
            {
                break;                
            }
        }
        if (count == 3)
        {
            TutorialFinish();
        }
    }

    private void TutorialFinish()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(-630, 230);
        MessageBox.TitleText.text = "ありがとうございます";
        MessageBox.MessageText.text = "おわいだ！がんばろう";
        MessageBox.NextButton.gameObject.SetActive(true);
    }
}
