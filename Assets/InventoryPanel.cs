using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryQuest.Components;
using InventoryQuest.Components.Entities.Player.Inventory;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;

public class InventoryPanel : MonoBehaviour
{
    public GameObject ItemPrefab;
    public int InventoryWidth = 8;
    [SerializeField]
    public float InventoryScale = 0.5f;
    private float _itemWidth;
    private float _itemHeight;

    private SortedList<int, ItemIcon> _ItemsPanel = new SortedList<int, ItemIcon>();

    /// <summary>
    ///     List of items on panel
    /// </summary>
    public SortedList<int, ItemIcon> ItemsPanel
    {
        get { return _ItemsPanel; }
        set { _ItemsPanel = value; }
    }

    void Start()
    {
        _itemWidth = ItemPrefab.GetComponent<RectTransform>().sizeDelta.x * InventoryScale;
        _itemHeight = ItemPrefab.GetComponent<RectTransform>().sizeDelta.y * InventoryScale;

        Inventory.EventItemAdded += Inventory_EventItemAdded;
        Inventory.EventItemDeleted += Inventory_EventItemDeleted;

        for (int i = 0; i < 20; i++)
        {
            CurrentGame.Instance.Player.Inventory.AddItem(RandomItemFactory.CreateItem(CurrentGame.Instance.Spot, EnumItemRarity.Normal));
        }
    }


    /*
     * All of those are panel operations and non of that acctauli do anything with items it self yet
     * CONSIDE THIS WHILE MORE CODE MANIPULATIONS
    */
    #region PanelManupilations
    /// <summary>
    /// Add item from other control to panel
    /// </summary>
    /// <param name="itemIcon"></param>
    public void AddToPanel(ItemIcon itemIcon)
    {
        itemIcon.transform.SetParent(ItemIcon.Inventory.transform);
        itemIcon.transform.localScale = new Vector3(InventoryScale, InventoryScale);
        ResizeInventoryPanel();
    }

    /// <summary>
    /// Input ItemIcon to recive its position on GUI
    /// TODO if eq is closed and item is pulled of grid?
    /// </summary>
    /// <param name="itemIcon"></param>
    /// <returns></returns>
    public int ResolvePosition(ItemIcon itemIcon)
    {

        var position = itemIcon.RectTransform.anchoredPosition;//+= new Vector2(ItemWidth, -ItemHeight);

        int index = 0;

        if (position.x < 0)
        {
            position.x = 0;
        }
        if (-position.y < 0)
        {
            position.y = 0;
        }
        int positionX = (int)(position.x / _itemWidth);
        int positionY = (int)(-position.y / _itemHeight);

        index = (positionY * InventoryWidth) + positionX;
        return index;
    }

    /// <summary>
    /// Take ItemIcon (size) and return current position in equipment
    /// TODO took resized element? Always change to default after drag
    /// TODO Set position in equipment
    /// </summary>
    /// <param name="itemIcon"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector2 SetPosition(ItemIcon itemIcon, int index)
    {
        if (index < 0)
        {
            //TODO equipment
            return new Vector2();
        }
        else
        {

            int indexW = index % InventoryWidth;
            int indexH = index / InventoryWidth;

            float x = indexW * _itemWidth + _itemWidth / 2f;
            float y = -indexH * _itemHeight - _itemHeight / 2f;

            return new Vector2(x, y);
        }
    }

    /// <summary>
    /// Swap out 2 ItemIcons in specific index if there is any
    /// </summary>
    /// <param name="itemIcon">Current item</param>
    /// <param name="newKey">New position</param>
    public void SwapItemsOnPanel(ItemIcon itemIcon, int newKey)
    {
        var inventory = CurrentGame.Instance.Player.Inventory;
        if (newKey < 0)
        {
            if (newKey == -(int)itemIcon.ItemData.ValidSlot)
            {
                //TODO equipment required
            }
            else
            {
                //SetPosition(itemIcon, itemIcon.ItemData.Index);
            }
        }
        else
        {
            ItemIcon inventoryItem;
            int oldKey = ItemsPanel.Keys[ItemsPanel.IndexOfValue(itemIcon)];
            ItemsPanel.TryGetValue(newKey, out inventoryItem);
            if (inventoryItem == null)
            {
                ItemsPanel.Remove(oldKey);
                inventory.MoveItem(oldKey, newKey);
                ItemsPanel.Add(newKey, itemIcon);
                itemIcon.RectTransform.anchoredPosition = SetPosition(itemIcon, newKey);
            }
            else
            {
                ItemsPanel[newKey] = itemIcon;
                ItemsPanel[oldKey] = inventoryItem;
                itemIcon.RectTransform.anchoredPosition = SetPosition(itemIcon, newKey);
                inventoryItem.RectTransform.anchoredPosition = SetPosition(inventoryItem, oldKey);
                inventory.SawpItems(newKey, oldKey);
            }
        }
        ResizeInventoryPanel();
    }

    /// <summary>
    /// Take element from panel 
    /// !! Doesnt not remove it from inventory yet
    /// </summary>
    /// <param name="itemIcon"></param>
    public void RemoveFromPanel(ItemIcon itemIcon)
    {
        itemIcon.transform.SetParent(ItemIcon.Canvas.transform);
        ResizeInventoryPanel();
    }

    #endregion

    #region InventoryOperations
    /// <summary>
    /// Create new ItemIcon with ItemData and setted new position
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Inventory_EventItemAdded(object sender, EventItemArgs e)
    {
        _ItemsPanel.Add(e.Index, CreateItemIcon(e.Index, e.Item));
        ResizeInventoryPanel();
    }

    /// <summary>
    /// Remove object assosiaded with Player Inventory
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Inventory_EventItemDeleted(object sender, EventItemArgs e)
    {
        ItemIcon go;
        ItemsPanel.TryGetValue(e.Index, out go);
        Destroy(go.gameObject);
        ItemsPanel.RemoveAt(e.Index);
        ResizeInventoryPanel();
    }

    void ResizeInventoryPanel()
    {
        if (!ItemsPanel.Any()) return;
        var lastKey = ItemsPanel.Keys[ItemsPanel.Count - 1];
        if (lastKey < 3 * InventoryWidth)
        {
            ItemIcon.Inventory.sizeDelta = new Vector2(532, 476);
        }
        else
        {
            var height = Mathf.CeilToInt(lastKey / (float)InventoryWidth);
            Debug.Log(height);
            ItemIcon.Inventory.sizeDelta = new Vector2(532, 35 + _itemHeight * height);
        }
    }

    #endregion

    #region ItemsIconFactory

    /// <summary>
    /// Return new ItemIcon object
    /// TODO Move to static factory?
    /// </summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public ItemIcon CreateItemIcon(int index, Item item)
    {
        ItemIcon.SetReferences();
        var itemIcon = Instantiate(ItemPrefab).GetComponent<ItemIcon>();
        itemIcon.ItemData = item;
        AddToPanel(itemIcon);
        itemIcon.RectTransform.anchoredPosition = SetPosition(itemIcon, index);
        return itemIcon;
    }

    #endregion
}
