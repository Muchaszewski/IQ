using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using InventoryQuest.Components;
using InventoryQuest.Game;
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
        SetIcon(CurrentGame.Instance.Spot.ImageString);
        GetComponent<Button>().onClick.AddListener(() => AreaButtonController.ChangeSpot());
    }

    public void SetIcon(string icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            _image.sprite = ResourceManager.Get("Sprites/gui/missingImage_0");
            return;
        }
        _image.sprite = ResourceManager.Get("Sprites/landscapes/" + icon + "_icon");
    }
}
