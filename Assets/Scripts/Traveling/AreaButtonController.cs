using UnityEngine;
using System.Collections;
using Extensions;
using UnityEngine.UI;
using InventoryQuest.Game;
using InventoryQuest.Components;
using InventoryQuest.InventoryQuest.Components.ActionEvents;

public class AreaButtonController : MonoBehaviour
{
    public Spot Spot { get; set; }
    private Text _spotName;
    private Text _spotLevel;
    public RectTransform RectTransform { get; set; }

    public static AreaImageController controller;

    private bool isChangeingSpot;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        if (controller == null)
        {
            controller = GameObject.FindObjectOfType<AreaImageController>();
        }
    }

    // Use this for initialization
    void Start()
    {
        _spotName = transform.GetChild(0).GetComponent<Text>();
        _spotLevel = transform.GetChild(1).GetComponent<Text>();
        //var _spotCat = transform.GetChild(2).GetComponent<Text>();
        //_spotCat.text = Spot.Category;

        _spotName.text = Spot.Name;
        _spotLevel.text = "Level " + Spot.Level.ToString();
        GetComponent<ExtendedButton>().onClick.AddListener(ChangeSpot);
        ActionEventManager.Fight.OnTravelEnd += CurrentGame_TravelingFinished;
    }

    private void CurrentGame_TravelingFinished(object sender, System.EventArgs e)
    {
        if (isChangeingSpot)
        {
            controller.ChangeBackground(Spot.ImageString);
            isChangeingSpot = false;
        }
    }

    // Update is called once per frame
    public void ChangeSpot()
    {
        CurrentGame.Instance.TravelToSpot = Spot.ID;
        CurrentGame.Instance.InvokeChangeSpot();
        isChangeingSpot = true;
    }

}
