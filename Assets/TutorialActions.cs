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
    [System.Serializable]
    public class TutorialMessage
    {
        public Vector2 position;
        public Vector2 size = new Vector2(500, 300);
        public string title;
        public string text;
    }

    // Editor variables
    public static TutorialActions Instance { get; set; }

    public MessageBox MessageBox;

    public Button[] MenuButtons;
    public Button ApplyStatisticsButton;

    public TutorialMessage introductionMessage;
    public TutorialMessage automaticCombatMessage;
    public TutorialMessage manualCombatMessage;
    public TutorialMessage rollStatsMessage;
    public TutorialMessage equipItemsMessage;
    public TutorialMessage statsPanelMessage;
    public TutorialMessage itemsPanelMessage;
    public TutorialMessage travellingMessage;
    public TutorialMessage travellingUseMessage;
    public TutorialMessage tutorialFinishMessage;

    // Private variables
    private int _attackCount = 0;



    // Use this for initialization
    void Start()
    {
        Instance = this;
        for (int i = 0; i < MenuButtons.Length - 1; i++)
        {
            MenuButtons[i].gameObject.SetActive(false);
        }
        MessageBox.SkipButton.onClick.AddListener(FinishTutorial);
#if !UNITY_EDITOR
        MessageBox.SkipButton.gameObject.SetActive(false);
#endif
    }

    void Update()
    {

    }

    public void TutorialIntroduction()
    {
        CurrentGame.Instance.FightController.Resume();
        MessageBox.RectTransform.anchoredPosition = introductionMessage.position;
        MessageBox.RectTransform.sizeDelta = introductionMessage.size;

        MessageBox.TitleText.text = introductionMessage.title;
        MessageBox.MessageText.text = introductionMessage.text;
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(TutorialAutoKill);
    }

    public void TutorialAutoKill()
    {
        MessageBox.RectTransform.anchoredPosition = automaticCombatMessage.position;
        MessageBox.RectTransform.sizeDelta = automaticCombatMessage.size;

        MessageBox.TitleText.text = automaticCombatMessage.title;
        MessageBox.MessageText.text = automaticCombatMessage.text;
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
        MessageBox.RectTransform.anchoredPosition = manualCombatMessage.position;
        MessageBox.RectTransform.sizeDelta = manualCombatMessage.size;

        MessageBox.TitleText.text = manualCombatMessage.title;
        MessageBox.MessageText.text = manualCombatMessage.text;
        MessageBox.NextButton.gameObject.SetActive(false);
        FightController.onAttack += FightController_onAttack;
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(TutorialOpenStats);
    }

    private void FightController_onAttack(object sender, FightControllerEventArgs e)
    {
        if (e.Invoker == e.FightController.Player)
        {
            _attackCount++;
        }
        if (_attackCount == 25)
        {
            MessageBox.NextButton.gameObject.SetActive(true);
            FightController.onAttack -= FightController_onAttack;
        }
    }

    private void TutorialOpenStats()
    {
        MessageBox.RectTransform.anchoredPosition = statsPanelMessage.position;
        MessageBox.RectTransform.sizeDelta = statsPanelMessage.size;

        MessageBox.TitleText.text = statsPanelMessage.title;
        MessageBox.MessageText.text = statsPanelMessage.text;
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(TutorialAutoKill);
        MessageBox.NextButton.gameObject.SetActive(false);

        MenuButtons[0].gameObject.SetActive(true);
        MenuButtons[0].onClick.AddListener(TutorialSetCharacter);
    }

    private void TutorialSetCharacter()
    {
        MenuButtons[0].onClick.RemoveListener(TutorialSetCharacter);

        MessageBox.RectTransform.anchoredPosition = rollStatsMessage.position;
        MessageBox.RectTransform.sizeDelta = rollStatsMessage.size;

        MessageBox.TitleText.text = rollStatsMessage.title;
        MessageBox.MessageText.text = rollStatsMessage.text;
        MessageBox.NextButton.gameObject.SetActive(false);

    }

    public void TutorialEquipmentOpen()
    {
        MessageBox.SkipButton.gameObject.SetActive(true);
        MenuButtons[4].gameObject.SetActive(true);
        MenuButtons[4].onClick.AddListener(TutorialEquipItems);

        MessageBox.RectTransform.anchoredPosition = itemsPanelMessage.position;
        MessageBox.RectTransform.sizeDelta = itemsPanelMessage.size;

        MessageBox.TitleText.text = itemsPanelMessage.title;
        MessageBox.MessageText.text = itemsPanelMessage.text;
        MessageBox.NextButton.gameObject.SetActive(false);
    }


    private void TutorialEquipItems()
    {
        MenuButtons[4].onClick.RemoveListener(TutorialEquipItems);

        MessageBox.RectTransform.anchoredPosition = equipItemsMessage.position;
        MessageBox.RectTransform.sizeDelta = equipItemsMessage.size;

        MessageBox.TitleText.text = equipItemsMessage.title;
        MessageBox.MessageText.text = equipItemsMessage.text;
        MessageBox.NextButton.gameObject.SetActive(false);
        Inventory.EventItemSwaped += Inventory_EventItemSwaped;
        Inventory.EventItemAdded += Inventory_EventItemSwaped;
        Inventory.EventItemDeleted += Inventory_EventItemSwaped;
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
            TutorialTraveling();
            Inventory.EventItemSwaped -= Inventory_EventItemSwaped;
            Inventory.EventItemAdded -= Inventory_EventItemSwaped;
            Inventory.EventItemDeleted -= Inventory_EventItemSwaped;
        }
    }

    private void TutorialTraveling()
    {
        MessageBox.RectTransform.anchoredPosition = travellingMessage.position;
        MessageBox.TitleText.text = travellingMessage.title;
        MessageBox.MessageText.text = travellingMessage.text;
        MessageBox.RectTransform.sizeDelta = travellingMessage.size;
        MessageBox.NextButton.gameObject.SetActive(false);
        MenuButtons[3].gameObject.SetActive(true);
        MenuButtons[3].onClick.AddListener(TutorialUseTraveling);
    }

    private void TutorialUseTraveling()
    {
        MessageBox.RectTransform.anchoredPosition = travellingUseMessage.position;
        MessageBox.TitleText.text = travellingUseMessage.title;
        MessageBox.MessageText.text = travellingUseMessage.text;
        MessageBox.RectTransform.sizeDelta = travellingUseMessage.size;
        MessageBox.NextButton.gameObject.SetActive(false);
        MenuButtons[3].onClick.RemoveListener(TutorialUseTraveling);
        CurrentGame.TravelingFinished += CurrentGame_TravelingFinished;
    }

    private void CurrentGame_TravelingFinished(object sender, System.EventArgs e)
    {
        CurrentGame.TravelingFinished -= CurrentGame_TravelingFinished;
        TutorialFinish();
    }

    private void TutorialFinish()
    {
        MessageBox.RectTransform.anchoredPosition = tutorialFinishMessage.position;
        MessageBox.TitleText.text = tutorialFinishMessage.title;
        MessageBox.MessageText.text = tutorialFinishMessage.text;
        MessageBox.RectTransform.sizeDelta = tutorialFinishMessage.size;
        MessageBox.NextButton.gameObject.SetActive(true);
        MessageBox.NextButton.onClick.AddListener(FinishTutorial);
    }

    /// <summary>
    /// 
    /// </summary>
    void FinishTutorial()
    {
        MessageBox.gameObject.SetActive(false);
        for (int i = 0; i < MenuButtons.Length - 1; i++)
        {
            MenuButtons[i].gameObject.SetActive(true);
        }
    }
}
