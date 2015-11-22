using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using InventoryQuest.Components;
using InventoryQuest.Game;
using InventoryQuest.InventoryQuest.Components.ActionEvents;
using UnityEngine.UI;

public class AreaIcon : MonoBehaviour
{
    [ReadOnly]
    public string Name;
    [ReadOnly]
    public string Category;
    [ReadOnly]
    public Vector3 Position;
    [ReadOnly]
    public float Size;

    public Color SelectedColor = Color.white;
    public Color DiscoveredColor = Color.gray;

    public Spot Spot { get; set; }

    private Image _image;
    private TooltipTrigger _tooltip;

    public TooltipTrigger Tooltip
    {
        get { return _tooltip; }
        set { _tooltip = value; }
    }

    public AreaButtonController AreaButtonController { get; set; }


    // Use this for initialization
    void Start()
    {
        _tooltip = GetComponent<TooltipTrigger>();
        _image = GetComponent<Image>();
        SetIcon(Spot.ImageString);
        GetComponent<Button>().onClick.AddListener(() => AreaButtonController.ChangeSpot());
        ActionEventManager.Fight.OnTravelEnd += OnTravelEnd;
    }

    void OnDestroy()
    {
        ActionEventManager.Fight.OnTravelEnd -= OnTravelEnd;
    }

    private void OnTravelEnd(object sender, EventArgs eventArgs)
    {
        _image.color = CurrentGame.Instance.Spot == Spot ? SelectedColor : DiscoveredColor;
    }

    public void SetIcon(string icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            _image.sprite = ResourceManager.Get("Sprites/gui/missingImage_0");
            return;
        }
        _image.sprite = ResourceManager.Get("Sprites/landscapes/" + icon + "_icon");

        _image.color = CurrentGame.Instance.Spot == Spot ? SelectedColor : DiscoveredColor;
    }
}
