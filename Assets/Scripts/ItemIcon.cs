using UnityEngine;
using System.Collections;
using System.Linq;
using InventoryQuest;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Game;
using InventoryQuest.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public InventoryQuest.Components.Items.Item ItemData { get; set; }

    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform != null)
            {
                return _rectTransform;
            }
            else
            {
                _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
    }

    private RectTransform _rectTransform;

    public static RectTransform Panel;
    public static RectTransform Inventory;
    public static Canvas Canvas;

    // Use this for initialization
    void Start()
    {
        if (ItemData == null)
        {
            Debug.LogError("Item data is required but was null");
        }
        SetIcon();
    }

    public static void SetReferences()
    {
        if (Canvas == null)
        {
            Inventory = FindObjectOfType<InventoryPanel>().GetComponent<RectTransform>();
            Panel = Inventory.parent.GetComponent<RectTransform>();
            Canvas = FindObjectOfType<Canvas>();
        }
    }

    void SetIcon()
    {
        var u = ImagesNames.ItemsImageNames[ItemData.ImageID.ImageIDType].FullNameList[ItemData.ImageID.ImageIDItem];
        //TODO Take code from work :P
        var sprite = Resources.Load<Sprite>(FileUtility.AssetsRelativePath(u));
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var inventory = Inventory.GetComponent<InventoryPanel>();
            this.transform.position = Input.mousePosition;
            inventory.RemoveFromPanelAndResize(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var inventory = Inventory.GetComponent<InventoryPanel>();
        int key = inventory.ResolvePosition(this);
        inventory.SwapItemsOnPanel(this, key);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var inventory = Inventory.GetComponent<InventoryPanel>();
            int key = inventory.GetEquipment(this);
            inventory.SwapItemsOnPanel(this, key);
        }
    }
}
