using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventoryQuest.Components.Entities.Player.Inventory;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;
using UnityEngine.Events;
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
    public Image TutorialButtonHighlight;

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
        SetTutorialMessage(introductionMessage, true, TutorialAutoKill);
    }

    public void TutorialAutoKill()
    {
        SetTutorialMessage(automaticCombatMessage, false, TutorialKillMonsters);
        FightController.onVicotry += FightController_onVicotry;
    }

    private void FightController_onVicotry(object sender, FightControllerEventArgs e)
    {
        MessageBox.NextButton.gameObject.SetActive(true);
        FightController.onVicotry -= FightController_onVicotry;
    }

    public void TutorialKillMonsters()
    {
        SetTutorialMessage(manualCombatMessage, false, TutorialOpenStats);
        FightController.onAttack += FightController_onAttack;
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
        SetTutorialMessage(statsPanelMessage, false, TutorialAutoKill);
        SetButtonHighlight(MenuButtons[0]);
        MenuButtons[0].gameObject.SetActive(true);
        MenuButtons[0].onClick.AddListener(TutorialSetCharacter);
    }

    private void TutorialSetCharacter()
    {
        MenuButtons[0].onClick.RemoveListener(TutorialSetCharacter);
        DisableButtonHighlight();

        SetTutorialMessage(rollStatsMessage);

    }

    public void TutorialEquipmentOpen()
    {
        MessageBox.SkipButton.gameObject.SetActive(true);
        SetButtonHighlight(MenuButtons[4]);
        MenuButtons[4].gameObject.SetActive(true);
        MenuButtons[4].onClick.AddListener(TutorialEquipItems);

        SetTutorialMessage(itemsPanelMessage);
    }


    private void TutorialEquipItems()
    {
        MenuButtons[4].onClick.RemoveListener(TutorialEquipItems);
        DisableButtonHighlight();

        SetTutorialMessage(equipItemsMessage);
        Inventory.EventItemSwaped += Inventory_EventItemSwaped;
        Inventory.EventItemAdded += Inventory_EventItemSwaped;
        Inventory.EventItemDeleted += Inventory_EventItemSwaped;
        Inventory.EventItemMoved += Inventory_EventItemSwaped;
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
            Inventory.EventItemMoved -= Inventory_EventItemSwaped;
        }
    }

    private void TutorialTraveling()
    {
        SetTutorialMessage(travellingMessage);
        MenuButtons[3].gameObject.SetActive(true);
        MenuButtons[3].onClick.AddListener(TutorialUseTraveling);
        SetButtonHighlight(MenuButtons[3]);
    }

    private void TutorialUseTraveling()
    {
        SetTutorialMessage(travellingUseMessage);
        MenuButtons[3].onClick.RemoveListener(TutorialUseTraveling);
        DisableButtonHighlight();
        CurrentGame.TravelingFinished += CurrentGame_TravelingFinished;
    }

    private void CurrentGame_TravelingFinished(object sender, System.EventArgs e)
    {
        CurrentGame.TravelingFinished -= CurrentGame_TravelingFinished;
        TutorialFinish();
    }

    private void TutorialFinish()
    {
        SetTutorialMessage(tutorialFinishMessage, true, FinishTutorial);
    }

    /// <summary>
    /// 
    /// </summary>
    void FinishTutorial()
    {
        DisableButtonHighlight();
        MessageBox.gameObject.SetActive(false);
        for (int i = 0; i < MenuButtons.Length - 1; i++)
        {
            MenuButtons[i].gameObject.SetActive(true);
        }
    }

    void SetTutorialMessage(TutorialMessage message, bool isNextActive = false, UnityAction action = null)
    {
        MessageBox.RectTransform.sizeDelta = message.size;
        MessageBox.RectTransform.anchoredPosition = message.position;
        MessageBox.TitleText.text = message.title;
        MessageBox.MessageText.text = message.text;
        MessageBox.RectTransform.sizeDelta = message.size;
        MessageBox.NextButton.gameObject.SetActive(isNextActive);
        MessageBox.NextButton.onClick.RemoveAllListeners();
        MessageBox.NextButton.onClick.AddListener(action);
    }

    void SetButtonHighlight(Button buttonToHighlight)
    {
        TutorialButtonHighlight.gameObject.SetActive(true);
        TutorialButtonHighlight.transform.position = buttonToHighlight.transform.position;
    }

    void DisableButtonHighlight()
    {
        TutorialButtonHighlight.gameObject.SetActive(false);
    }
}
