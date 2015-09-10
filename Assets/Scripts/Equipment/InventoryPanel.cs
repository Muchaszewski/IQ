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
    public GameObject SellItemArea;
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
            CurrentGame.Instance.Player.Inventory.AddItem(RandomItemFactory.CreateItem(CurrentGame.Instance.Spot, EnumItemRarity.Mythical));
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
    public int? ResolvePosition(ItemIcon itemIcon)
    {
        int index = ItemsPanel.IndexOfValue(itemIcon);
        var key = ItemsPanel.Keys[index];

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
            return null;
        }

        //Change paretn to inventory for easier position controll
        AddToPanel(itemIcon);
        var position = itemIcon.RectTransform.anchoredPosition;
        RemoveFromPanel(itemIcon);
        //Proper adding is called later so remove it for good sake
        int positionX = (int)(position.x / _itemWidth);
        int positionY = (int)(-position.y / _itemHeight);

        return (positionY * InventoryWidth) + positionX;
    }

    /// <summary>
    /// Return equipment slot with index ready value
    /// Return 0 if something broke?
    /// </summary>
    /// <param name="itemIcon"></param>
    /// <returns></returns>
    public int GetEquipment(ItemIcon itemIcon)
    {
        for (int i = 0; i < _equipment.Count; i++)
        {
            if (_equipment[i].Slot == itemIcon.ItemData.ValidSlot)
            {
                return -i - 1;
            }
        }
        return 0;
    }

    /// <summary>
    /// Take ItemIcon (size) and return current position in equipment
    /// </summary>
    /// <param name="itemIcon"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public Vector2 GetPosition(int key)
    {
        if (key < 0)
        {
            //For equipment return zero
            //and resolve it while setting
            return Vector2.zero;
        }
        else
        {
            int indexW = key % InventoryWidth;
            int indexH = key / InventoryWidth;

            float x = indexW * _itemWidth + _itemWidth / 2f;
            float y = -indexH * _itemHeight - _itemHeight / 2f;

            return new Vector2(x, y);
        }
    }

    public void SetPositon(ItemIcon itemIcon, int key, Vector2 position)
    {
        if (key < 0)
        {
            //Return negative index of equipment 
            //Look ResolvePostion
            var eq = _equipment[-(key + 1)];
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

    public void GetAndSetPosition(ItemIcon itemIcon, int key)
    {
        SetPositon(itemIcon, key, GetPosition(key));
    }

    /// <summary>
    /// Swap out 2 ItemIcons in specific index if there is any
    /// </summary>
    /// <param name="itemIcon">Current item</param>
    /// <param name="newKey">New position</param>
    public void SwapItemsOnPanel(ItemIcon itemIcon, int? givenKey)
    {
        int newKey;
        int index = ItemsPanel.IndexOfValue(itemIcon);
        if (givenKey == null)
        {
            newKey = ItemsPanel.Keys[index];
        }
        else
        {
            newKey = givenKey.Value;
        }

        var inventory = CurrentGame.Instance.Player.Inventory;
        //Get old key
        int oldKey = ItemsPanel.Keys[index];
        //Get item to swap (probably)
        ItemIcon inventoryItem;
        ItemsPanel.TryGetValue(newKey, out inventoryItem);

        //Dont put item if it is level above player
        if (CurrentGame.Instance.Player.Level < itemIcon.ItemData.RequiredLevel && givenKey < 0)
        {
            GetAndSetPosition(itemIcon, oldKey);
            return;
        }

        //BUG If swap out from equipment to inventory 
        //Implemented special behavior
        if (inventoryItem != null)
        {
            if (oldKey < 0)
            {
                if (_equipment[-(oldKey + 1)].Slot != inventoryItem.ItemData.ValidSlot)
                {
                    GetAndSetPosition(itemIcon, oldKey);
                    return;
                }
                TakeOutItemFromSlot(itemIcon);
                return;
            }
        }

        if (inventoryItem == null)
        {
            //Get current item and remove it from unity list
            ItemsPanel.Remove(oldKey);
            //Swap item in dll to match new setup
            inventory.MoveItem(oldKey, newKey);
            //Add again item at new position to match dll new setup
            ItemsPanel.Add(newKey, itemIcon);
            //Put new item in Unity space
            GetAndSetPosition(itemIcon, newKey);
        }
        else if (inventoryItem == itemIcon)
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
        if (newKey < 0)
        {
            //If TwoHanded is beeing performed to equip
            if (itemIcon.ItemData.RequiredHands == EnumItemHands.TwoHanded)
            {
                var shieldSlotIndex = _equipment.FindIndex(x => x.Slot == EnumItemSlot.OffHand);
                var shield = ItemsPanel.FirstOrDefault(x => x.Key == -shieldSlotIndex - 1);
                if (shield.Value != null)
                {
                    var free = CurrentGame.Instance.Player.Inventory.GetNewIndex();
                    ItemsPanel.Remove(-shieldSlotIndex - 1);
                    inventory.MoveItem(-shieldSlotIndex - 1, free);
                    ItemsPanel.Add(free, shield.Value);
                    GetAndSetPosition(shield.Value, free);
                    ResizeItemIcon(shield.Value);
                }
            }

            //If Shield is perform to equip and you already have two handed weapon
            if (itemIcon.ItemData.ValidSlot == EnumItemSlot.OffHand)
            {
                int weaponSlotIndex = _equipment.FindIndex(x => x.Slot == EnumItemSlot.Weapon);
                var weapon = ItemsPanel.FirstOrDefault(x => x.Key == -weaponSlotIndex - 1);
                if (weapon.Value != null)
                {
                    if (weapon.Value.ItemData.RequiredHands == EnumItemHands.TwoHanded)
                    {
                        var free = CurrentGame.Instance.Player.Inventory.GetNewIndex();
                        ItemsPanel.Remove(-weaponSlotIndex - 1);
                        inventory.MoveItem(-weaponSlotIndex - 1, free);
                        ItemsPanel.Add(free, weapon.Value);
                        GetAndSetPosition(weapon.Value, free);
                        ResizeItemIcon(weapon.Value);
                    }
                }
            }
        }

        ResizeInventoryPanel();
    }

    public void TakeOutItemFromSlot(ItemIcon itemIcon)
    {
        int oldKey = ItemsPanel.Keys[ItemsPanel.IndexOfValue(itemIcon)];
        if (oldKey < 0)
        {
            var inventory = CurrentGame.Instance.Player.Inventory;
            var free = CurrentGame.Instance.Player.Inventory.GetNewIndex();
            ItemsPanel.Remove(oldKey);
            inventory.MoveItem(oldKey, free);
            ItemsPanel.Add(free, itemIcon);
            GetAndSetPosition(itemIcon, free);
            ResizeItemIcon(itemIcon);
        }
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
            ItemIcon.Inventory.GetComponent<RectTransform>().sizeDelta = new Vector2(532, 476);
        }
        else
        {
            var height = Mathf.FloorToInt(lastKey / (float)InventoryWidth);
            ItemIcon.Inventory.GetComponent<RectTransform>().sizeDelta = new Vector2(532, InventoryNewLineSpace + _itemHeight * height);
        }
    }

    public void ClearInventory()
    {
        foreach (var item in ItemsPanel)
        {
            Destroy(item.Value.gameObject);
        }
        ItemsPanel = new SortedList<int, ItemIcon>();
    }

    public void PopulateInventory()
    {
        ClearInventory();
        foreach (var item in CurrentGame.Instance.Player.Inventory.Items)
        {
            _ItemsPanel.Add(item.Key, CreateItemIcon(item.Key, item.Value));
            ResizeInventoryPanel();
        }
    }

    /// <summary>
    /// Sort inventory with given comparer
    /// </summary>
    public void SortInventory()
    {
        var items = CurrentGame.Instance.Player.Inventory.Items;
        //Restrict sorting to inventory only (extract equipment)
        var equipment = items.Where(x => x.Key < 0).ToList();
        //Remote equipment from list
        foreach (var item in equipment)
        {
            items.Remove(item.Key);
        }
        //Pass current item list for easier manipulations
        var list = items.Values.ToList();
        //Sort new list
        list.Sort(new InventoryConparer<Item>());
        //Clear old list and fill with new items sorted
        CurrentGame.Instance.Player.Inventory.Items.Clear();
        foreach (var item in equipment)
        {
            items.Add(item.Key, item.Value);
        }
        for (int i = 0; i < list.Count; i++)
        {
            items.Add(i, list[i]);
        }
        //Set all to display
        PopulateInventory();
    }

    /// <summary>
    /// Comparer for sorting invetnory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class InventoryConparer<T> : IComparer<T> where T : Item
    {
        public int Compare(T x, T y)
        {
            int result = y.Rarity.CompareTo(x.Rarity);
            if (result != 0) { return result; }
            result = x.Type.CompareTo(y.Type);
            if (result != 0) { return result; }
            return x.ItemLevel.CompareTo(y.ItemLevel);
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
