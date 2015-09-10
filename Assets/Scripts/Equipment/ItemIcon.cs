using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Utils;
using InventoryQuest;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Components.Items;
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
    public static InventoryPanel Inventory;
    public static Canvas Canvas;

    // Use this for initialization
    void Start()
    {
        if (ItemData == null)
        {
            Debug.LogError("Item data is required but was null");
        }
        SetIcon();
        SetBackground();
    }

    public void OnDestroy()
    {
        GetComponent<ShowTooltip>().ToolTipManager.RemoveTooltipReference();
    }

    public static void SetReferences()
    {
        if (Canvas == null)
        {
            Inventory = FindObjectOfType<InventoryPanel>().GetComponent<InventoryPanel>();
            Panel = Inventory.transform.parent.GetComponent<RectTransform>();
            Canvas = FindObjectOfType<Canvas>();
        }
    }

    /// <summary>
    /// Set icon for given image
    /// </summary>
    void SetIcon()
    {
        var path = ImagesNames.ItemsImageNames[ItemData.ImageID.ImageIDType].FullNameList[ItemData.ImageID.ImageIDItem];
        transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Get(FileUtility.AssetsRelativePath(path));
    }

    void SetBackground()
    {
        string path = ItemBackgrounds.Get(ItemData.Rarity);
        GetComponent<Image>().sprite = ResourceManager.Get(path);
    }

    public void Sell()
    {

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
        int? positionKey = Inventory.ResolvePosition(this);
        if (positionKey == null)
        {
            Rect rect = new Rect(
                Inventory.SellItemArea.transform.position.x,
                Inventory.SellItemArea.transform.position.y,
                Inventory.SellItemArea.GetComponent<RectTransform>().sizeDelta.x * Inventory.SellItemArea.transform.localScale.x,
                Inventory.SellItemArea.GetComponent<RectTransform>().sizeDelta.y * Inventory.SellItemArea.transform.localScale.y
                );
            if (rect.Contains(eventData.position))
            {
                /*
                ArgumentException: element already exists
                System.Collections.Generic.SortedList`2[System.Int32,InventoryQuest.Components.Items.Item].PutImpl (Int32 key, InventoryQuest.Components.Items.Item value, Boolean overwrite)
                System.Collections.Generic.SortedList`2[System.Int32,InventoryQuest.Components.Items.Item].Add (Int32 key, InventoryQuest.Components.Items.Item value)
                InventoryQuest.Components.Entities.Player.Inventory.Inventory.MoveItem (Int32 indexFrom, Int32 indexWhere) (at InventoryQuest/InventoryQuest/Components/Entities/Player/Inventory/Inventory.cs:116)
                InventoryPanel.SwapItemsOnPanel (.ItemIcon itemIcon, Nullable`1 givenKey) (at Assets/Scripts/Equipment/InventoryPanel.cs:245)
                ItemIcon.OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData) (at Assets/Scripts/Equipment/ItemIcon.cs:99)
                UnityEngine.EventSystems.ExecuteEvents.Execute (IEndDragHandler handler, UnityEngine.EventSystems.BaseEventData eventData) (at C:/buildslave/unity/build/Extensions/guisystem/UnityEngine.UI/EventSystem/ExecuteEvents.cs:80)
                UnityEngine.EventSystems.ExecuteEvents.Execute[IEndDragHandler] (UnityEngine.GameObject target, UnityEngine.EventSystems.BaseEventData eventData, UnityEngine.EventSystems.EventFunction`1 functor) (at C:/buildslave/unity/build/Extensions/guisystem/UnityEngine.UI/EventSystem/ExecuteEvents.cs:269)
                UnityEngine.EventSystems.EventSystem:Update()
                */
                //Remove all existence of this object in lists to allow GC work
                var index = CurrentGame.Instance.Player.Inventory.Items.IndexOfValue(ItemData);
                var key = CurrentGame.Instance.Player.Inventory.Items.Keys[index];
                Item item;
                CurrentGame.Instance.Player.Inventory.Items.TryGetValue(key, out item);
                CurrentGame.Instance.Player.Inventory.Items.RemoveAt(index);
                Inventory.ItemsPanel.RemoveAt(index);
                //Give money
                //Debug.Log("Shut up and take my money!");
                //Destroy sold item
                //TODO Store for rebuy
                Destroy(gameObject);
                return;
            }
        }
        Inventory.SwapItemsOnPanel(this, positionKey);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ;
            int key = Inventory.GetEquipment(this);
            Inventory.SwapItemsOnPanel(this, key);
        }
    }
}
