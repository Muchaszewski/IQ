using UnityEngine;
using System.Collections;
using System.Linq;
using InventoryQuest;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Game;
using InventoryQuest.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public InventoryQuest.Components.Items.Item ItemData { get; set; }

    public RectTransform RectTransform { get; private set; }

    public static RectTransform Panel;
    public static RectTransform Inventory;
    public static Canvas Canvas;

    // Use this for initialization
    void Start()
    {
        if (Canvas == null)
        {
            Inventory = transform.parent.GetComponent<RectTransform>();
            Panel = transform.parent.parent.GetComponent<RectTransform>();
            Canvas = FindObjectOfType<Canvas>();
        }
        var spot = CurrentGame.Instance.Spot;
        ItemData = RandomItemFactory.CreateItem(1, spot);
        SetIcon();
        RectTransform = GetComponent<RectTransform>();
    }

    void SetIcon()
    {
        var u = ImagesNames.ItemsImageNames[ItemData.ImageID.ImageIDType].FullNameList[ItemData.ImageID.ImageIDItem];
        //TODO Take code from work :P
        var sprite = Resources.Load<Sprite>(FileUtility.AssetsRelativePath(u));
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    public void Drag()
    {
        this.transform.position = Input.mousePosition;
        Inventory.GetComponent<InventoryPanel>().RemoveFromPanel(this);
    }

    public void Drop()
    {
        Inventory.GetComponent<InventoryPanel>().AddToPanel(this);

    }
}
