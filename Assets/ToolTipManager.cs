using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InventoryQuest.Utils;

public class ToolTipManager : UIManager
{

    //Show in inspector
    public Color[] RarityColors;
    public Image TooltipImage;

    //Properties
    public bool Show { get; set; }

    //Private varibles
    private RectTransform _rectTransform;
    private InventoryQuest.Components.Items.Item _item;

    private int _currentHeight = 0;
    //_____________________________________________________________________________________________________________

    [SerializeField]
    [HideInInspector]
    private List<GameObject> _tootipObjects = new List<GameObject>();

    /// <summary>
    /// List of tooltip objects current displayed
    /// </summary>
    public List<GameObject> TootipObjects
    {
        get { return _tootipObjects; }
        set { _tootipObjects = value; }
    }

    //__________________________________________________MonoBehaviour______________________________________________

    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Show || Input.GetMouseButton(0))
        {
            transform.position = new Vector3(-10000, -10000, 0);
        }
        else
        {
            transform.position = (Vector2)Input.mousePosition + new Vector2(-4, -4);
            Show = false;
        }
    }

    //______________________________________________End MonoBehaviour End__________________________________________

    //______________________________________________Public Methods_________________________________________________

    public void SetTooltip(InventoryQuest.Components.Items.Item item)
    {
        if (item != _item)
        {
            _item = item;
            foreach (var o in _tootipObjects)
            {
                Destroy(o);
            }
            _tootipObjects = new List<GameObject>();
            if (item != null)
            {
                SetIcon();
                //Do not use outside this method or in custom inspector methods
#pragma warning disable 618
                CreateLabels(item);
#pragma warning restore 618
            }
        }
    }

    void SetIcon()
    {
        var u = ImagesNames.ItemsImageNames[_item.ImageID.ImageIDType].FullNameList[_item.ImageID.ImageIDItem];
        //TODO Take code from work :P
        var sprite = Resources.Load<Sprite>(FileUtility.AssetsRelativePath(u));
        TooltipImage.sprite = sprite;
    }

    /// <summary>
    /// Create all labels for tooltip
    /// </summary>
    /// <param name="item"></param>
    [Obsolete("Only for inspector use, and SetTooltip method")]
    public void CreateLabels(InventoryQuest.Components.Items.Item item)
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        SetWindowSize(381);
        CreateHeader(item);
        CreateBasicMisc(item);
        //---------------
        CreateStats(item);
        CreateRequirements(item);
        CreateFlavor(item);
        //---------------
        CreateValue(item);

        AdjustTooltipSize();
    }

    public void SetWindowSize(float newHeight)
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, newHeight);
    }

    public void AddWindowSize(float height)
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + height);
    }

    public float GetWindowHeight()
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        return _rectTransform.sizeDelta.y;
    }

    //__________________________________________End Public Methods End_____________________________________________

    void CreateHeader(InventoryQuest.Components.Items.Item item)
    {
        if (item.ExtraName == null)
        {
            AddLabel(new Vector2(0, 30), item.Name, 35, TextAnchor.MiddleCenter);
        }
        else
        {
            AddLabel(new Vector2(0, 15), item.Name, TextAnchor.MiddleCenter);
            AddLabel(new Vector2(0, 42), item.ExtraName, TextAnchor.MiddleCenter);
        }
    }

    void CreateBasicMisc(InventoryQuest.Components.Items.Item item)
    {
        switch ( item.ValidSlot )
        {
            // Weapon
            case InventoryQuest.Components.Items.EnumItemSlot.Weapon:
                AddLabel(new Vector2(40, 86), item.RequiredHands + " " + item.Type.ToString(), 16, Color.gray);
                break;

            // Default: armor
            default:
                AddLabel(new Vector2(40, 86), item.ValidSlot.ToString() + " armor", 16, Color.gray);
                break;
        }

        // Rarity
        AddLabel(new Vector2(219, 334), item.Rarity.ToString(), 16, Color.gray, TextAnchor.UpperCenter);

    }

    void CreateStats(InventoryQuest.Components.Items.Item item)
    {

        int descriptionLeftMargin = 30;
        int valueLeftMargin = -150;
        int topMargin = 125;
        int normalFontSize = 24;
        int bigFontSize = 36;

        int smallGap = 4;
        int largeGap = 12;

        // Round up values that should not be floating-point type variables
        item.Durability.Current = (float) Math.Floor(item.Durability.Current);
        item.Durability.Base = (float)Math.Floor(item.Durability.Base);

        switch (item.ValidSlot)
        {
            // Weapon
            case InventoryQuest.Components.Items.EnumItemSlot.Weapon:

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Stat 1" + " " + "Stat 2", normalFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += normalFontSize + largeGap;

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Damage:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.MinDamage.ToString() + "-" + item.Stats.MaxDamage.ToString(), normalFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;
                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Speed:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.AttackSpeed.ToString(), normalFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += normalFontSize + largeGap;

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "DPS:", bigFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.DPS.ToString(), bigFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += bigFontSize + smallGap;
                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Critical:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.CriticalChance.ToString() + " (" + item.Stats.CriticalDamage.ToString()  + ")", normalFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += normalFontSize + largeGap;

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Range:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Range.ToString(), normalFontSize, Color.grey, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;
                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Accuracy:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.Accuracy.ToString(), normalFontSize, Color.grey, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;
                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Pierce:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.ArmorPenetration.ToString(), normalFontSize, Color.grey, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;
                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Parry:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.Deflection.ToString(), normalFontSize, Color.grey, TextAnchor.UpperLeft);
                topMargin += normalFontSize + largeGap;

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Durability:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Durability.Current.ToString() + "/" + item.Durability.Base.ToString(), normalFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;
                break;

            // Armor
            case InventoryQuest.Components.Items.EnumItemSlot.Back:
            case InventoryQuest.Components.Items.EnumItemSlot.Chest:
            case InventoryQuest.Components.Items.EnumItemSlot.Feet:
            case InventoryQuest.Components.Items.EnumItemSlot.Hands:
            case InventoryQuest.Components.Items.EnumItemSlot.Head:
            case InventoryQuest.Components.Items.EnumItemSlot.Legs:
            case InventoryQuest.Components.Items.EnumItemSlot.Shoulders:
            case InventoryQuest.Components.Items.EnumItemSlot.Waist:

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Type:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Skill.ToString(), normalFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Armor:", bigFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.Armor.ToString(), bigFontSize, Color.white, TextAnchor.UpperLeft);
                topMargin += bigFontSize + largeGap;

                AddLabel(new Vector2(descriptionLeftMargin, topMargin), "Durability:", normalFontSize, Color.gray, TextAnchor.UpperLeft);
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Durability.Current.ToString() + "/" + item.Durability.Base.ToString(), normalFontSize, Color.grey, TextAnchor.UpperLeft);
                topMargin += normalFontSize + smallGap;
                break;
        }

        _currentHeight = topMargin + 20;
    }

    void CreateRequirements(InventoryQuest.Components.Items.Item item)
    {
        if (_currentHeight < 360)
            _currentHeight = 360;
        int topMargin = _currentHeight;
        topMargin += 12;

        AddSeparator(new Vector2(0, topMargin));
        topMargin += 20;

        AddLabel(new Vector2(160, topMargin), "Level " + item.ItemLevel.ToString(), 20, Color.grey, TextAnchor.UpperLeft);
        AddLabel(new Vector2(-160, topMargin), "Required stats", 20, Color.grey, TextAnchor.UpperRight);
        topMargin += 28;

        _currentHeight = topMargin;
    }

    void CreateFlavor(InventoryQuest.Components.Items.Item item)
    {
        // The window often does not display correctly when this if case does not fire
        // I have no idea why.

        if (true)
        //if (item.FlavorText.Length > 0)
        {
            int topMargin = _currentHeight;
            topMargin += 12;

            AddSeparator(new Vector2(0, topMargin));
            topMargin += 20;

            AddLabel(new Vector2(-100, topMargin), item.FlavorText, 20, Color.grey, TextAnchor.UpperRight);
            AddLabel(new Vector2(0, topMargin), "Sample flavor text", 20, Color.grey, TextAnchor.UpperRight);
            topMargin += 28;

            _currentHeight = topMargin;
        }
    }

    void CreateValue(InventoryQuest.Components.Items.Item item)
    {
        //var relativeHeight = GetWindowHeight();

        int topMargin = _currentHeight;
        topMargin += 12;

        AddSeparator(new Vector2(0, topMargin));
        topMargin += 12;

        AddLabel(new Vector2(0, topMargin), item.Price.ToString(), 20, Color.grey, TextAnchor.MiddleCenter);
        topMargin += 22;

        _currentHeight = topMargin;
    }

    void AdjustTooltipSize()
    {
        // Does not seem to work all the time in the editor
        SetWindowSize(_currentHeight + 20);
    }

    //__________________________________________Overrided methods_____________________________________________


    protected override void CreateParent(Vector2 position, RectTransform rectTransform)
    {
        base.CreateParent(position, rectTransform);
        _tootipObjects.Add(rectTransform.gameObject);
    }
}
