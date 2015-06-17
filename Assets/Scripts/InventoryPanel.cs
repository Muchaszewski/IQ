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
    public static InventoryPanel Instance { get; set; }
    public GameObject ItemPrefab;
    public int InventoryWidth = 8;
    public float InventoryScale = 0.5f;
    [Tooltip("If screen is too small for displaying new items it will resize and this is additional space after last lane of items")]
    public float InventoryNewLineSpace = 35;
    private float _itemWidth;
    private float _itemHeight;

    private SortedList<int, ItemIcon> _ItemsPanel = new SortedList<int, ItemIcon>();
    private List<EquipmentSlot> _equipment = new List<EquipmentSlot>();

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
        Instance = this;
        _itemWidth = ItemPrefab.GetComponent<RectTransform>().sizeDelta.x * InventoryScale;
        _itemHeight = ItemPrefab.GetComponent<RectTransform>().sizeDelta.y * InventoryScale;
        _equipment.AddRange(FindObjectsOfType<EquipmentSlot>());

        Inventory.EventItemAdded += Inventory_EventItemAdded;
        Inventory.EventItemDeleted += Inventory_EventItemDeleted;

        for (int i = 0; i < 20; i++)
        {
            CurrentGame.Instance.Player.Inventory.AddItem(RandomItemFactory.CreateItem(CurrentGame.Instance.Spot, EnumItemRarity.Normal));
        }
    }

    void Update()
    {
        //Debug.Log(Input.mousePosition);
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
        ResizeInventoryPanel();
    }

    /// <summary>
    /// Input ItemIcon to recive its position on GUI
    /// </summary>
    /// <param name="itemIcon"></param>
    /// <returns></returns>
    public int ResolvePosition(ItemIcon itemIcon)
    {
        int index = 0;
        var key = ItemsPanel.Keys[ItemsPanel.IndexOfValue(itemIcon)];

        //If equipment
        for (int i = 0; i < _equipment.Count; i++)
        {
            if (_equipment[i].GetAbsolutiveRect().Contains(itemIcon.transform.position))
            {
                if (_equipment[i].Slot == itemIcon.ItemData.ValidSlot)
                {
                    //Return negative index of equipment
                    //Easier to manipulate with index then with abstract slot
                    //Also easier to manipulate if slot there is more then one type of slot
                    return -i - 1;
                }
                else
                {
                    return key;
                }
            }
        }
        //else if inventory
        Rect rect = new Rect(
            transform.position.x,
            transform.position.y,
            GetComponent<RectTransform>().sizeDelta.x * transform.localScale.x,
            GetComponent<RectTransform>().sizeDelta.y * transform.localScale.y
            );
        if (!rect.Contains(itemIcon.transform.position))
        {
            return key;
        }

        //Change paretn to inventory for easier position controll
        AddToPanel(itemIcon);
        var position = itemIcon.RectTransform.anchoredPosition;
        RemoveFromPanel(itemIcon);
        //Proper adding is called later so remove it for good sake
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
    public Vector2 GetPosition(int index)
    {
        if (index < 0)
        {
            //For equipment return zero
            //and resolve it while setting
            return Vector2.zero;
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

    public void SetPositon(ItemIcon itemIcon, int index, Vector2 position)
    {
        if (index < 0)
        {
            //Return negative index of equipment 
            //Look ResolvePostion
            var eq = _equipment[-(index + 1)];
            eq.AddToPanel(itemIcon);
            //Resolve position while setting because every setting in eq is diffrent
            eq.SetItemIcon(itemIcon);
        }
        else
        {
            AddToPanel(itemIcon);
            itemIcon.RectTransform.anchoredPosition = position;
        }
    }

    public void GetAndSetPosition(ItemIcon itemIcon, int index)
    {
        SetPositon(itemIcon, index, GetPosition(index));
    }

    /// <summary>
    /// Swap out 2 ItemIcons in specific index if there is any
    /// </summary>
    /// <param name="itemIcon">Current item</param>
    /// <param name="newKey">New position</param>
    public void SwapItemsOnPanel(ItemIcon itemIcon, int newKey)
    {
        var inventory = CurrentGame.Instance.Player.Inventory;

        ItemIcon inventoryItem;
        int oldKey = ItemsPanel.Keys[ItemsPanel.IndexOfValue(itemIcon)];
        ItemsPanel.TryGetValue(newKey, out inventoryItem);
        if (inventoryItem == null)
        {
            ItemsPanel.Remove(oldKey);
            inventory.MoveItem(oldKey, newKey);
            ItemsPanel.Add(newKey, itemIcon);
            GetAndSetPosition(itemIcon, newKey);
        }
        else
        {
            ItemsPanel[newKey] = itemIcon;
            ItemsPanel[oldKey] = inventoryItem;
            GetAndSetPosition(itemIcon, newKey);
            GetAndSetPosition(inventoryItem, oldKey);
            ResizeItemIcon(inventoryItem);
            inventory.SawpItems(newKey, oldKey);
        }
        ResizeInventoryPanel();
    }


    /// <summary>
    /// Move element to Canvas and resize it to userfrendly scale
    /// </summary>
    /// <param name="itemIcon"></param>
    public void RemoveFromPanelAndResize(ItemIcon itemIcon)
    {
        ResizeItemIcon(itemIcon);
        RemoveFromPanel(itemIcon);
    }

    /// <summary>
    /// Take element from panel 
    /// !! Doesnt not remove it from inventory yet
    /// </summary>
    /// <param name="itemIcon"></param>
    public void RemoveFromPanel(ItemIcon itemIcon)
    {
        //Add to Canvas for free movemnt
        itemIcon.transform.SetParent(ItemIcon.Canvas.transform);
        //Resize inventory panel (in canse it was in invetnory)
        ResizeInventoryPanel();
    }

    /// <summary>
    /// Resize element to userfrendly scale
    /// </summary>
    /// <param name="itemIcon"></param>
    public void ResizeItemIcon(ItemIcon itemIcon)
    {
        //Set userfrendly size of itemIcon
        itemIcon.RectTransform.sizeDelta = ItemPrefab.GetComponent<RectTransform>().sizeDelta;
        itemIcon.transform.localScale = Vector3.one * InventoryScale;
        var image = itemIcon.RectTransform.GetChild(0).GetComponent<RectTransform>();
        image.sizeDelta = ItemPrefab.GetComponent<RectTransform>().sizeDelta;
        image.transform.localScale = Vector3.one;
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
            ItemIcon.Inventory.sizeDelta = new Vector2(532, InventoryNewLineSpace + _itemHeight * height);
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
        itemIcon.transform.localScale = Vector3.one * InventoryScale;
        GetAndSetPosition(itemIcon, index);
        return itemIcon;
    }

    #endregion
}
