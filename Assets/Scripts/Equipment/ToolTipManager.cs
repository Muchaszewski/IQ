using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using InventoryQuest;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;
using InventoryQuest.Utils;
using Assets.Scripts;
using Assets.Scripts.Utils;
using InventoryQuest.Components.Entities.Player;

public class ToolTipManager : UILabelManager
{

    //Show in inspector
    public Color[] RarityColors;
    public Image TooltipImage;
    public Image TooltipBGImage;
    public Image TooltipHeaderImage;
    public Image Overlay;

    //Properties
    public bool Show { get; set; }
    private Color CurrentRarityColor { get; set; }
    public float Margin = 50;

    //Private varibles
    private RectTransform _rectTransform;
    private ItemIcon _item;
    private Canvas _canvas;

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
        _canvas = GameObject.FindObjectOfType<Canvas>();
        Player.LeveledUp += Player_LeveledUp;
    }

    private void Player_LeveledUp(object sender, EventArgs e)
    {
        if (CurrentGame.Instance.Player.Level >= _item.ItemData.RequiredLevel)
        {
            SetOverlay(false);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Show || Input.GetMouseButton(0) || _item == null)
        {
            transform.position = new Vector3(-10000, -10000, 0);
        }
        else 
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(365,418);

            Show = false;
        }
    }

    //______________________________________________End MonoBehaviour End__________________________________________

    //______________________________________________Public Methods_________________________________________________

    public void SetTooltip(ItemIcon item)
    {
        if (item != _item)
        {
            _item = item;
            foreach (var o in _tootipObjects)
            {
                Destroy(o);
            }
            _tootipObjects = new List<GameObject>();
            if (item.ItemData != null)
            {
                SetBackground();
                SetIcon();
                if (CurrentGame.Instance.Player.Level < _item.ItemData.RequiredLevel)
                {
                    SetOverlay(true);
                }
                else
                {
                    SetOverlay(false);
                }
                CurrentRarityColor = RarityColors[(int)_item.ItemData.Rarity];
                //Do not use outside this method or in custom inspector methods
#pragma warning disable 618
                CreateLabels(item.ItemData);
#pragma warning restore 618
            }
        }
    }

    void SetIcon()
    {
        var u = ImagesNames.ItemsImageNames[_item.ItemData.ImageID.ImageIDType].FullNameList[_item.ItemData.ImageID.ImageIDItem];
        //TODO Take code from work :P
        var sprite = ResourceManager.Get(FileUtility.AssetsRelativePath(u));
        TooltipImage.sprite = sprite;
    }

    void SetBackground()
    {
        string path = ItemBackgrounds.Get(_item.ItemData.Rarity);
        TooltipBGImage.sprite = ResourceManager.Get(path);

        path = ItemBackgrounds.GetHeader(_item.ItemData.Rarity);
        TooltipHeaderImage.sprite = ResourceManager.Get(path);
    }

    void SetOverlay(bool option)
    {
        Overlay.enabled = option;
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
        SetMagicAttributes(item);
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
        if (item.ExtraName == null || item.ExtraName == String.Empty)
        {
            AddLabel(new Vector2(0, 30), item.Name, 35, CurrentRarityColor, TextAnchor.MiddleCenter);
        }
        else
        {
            AddLabel(new Vector2(0, 15), item.Name, 20, CurrentRarityColor, TextAnchor.MiddleCenter);
            AddLabel(new Vector2(0, 42), item.ExtraName, 20, TextAnchor.MiddleCenter);
        }
    }

    void CreateBasicMisc(InventoryQuest.Components.Items.Item item)
    {
        switch (item.ValidSlot)
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
        item.Durability.Current = (float)Math.Floor(item.Durability.Current);
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
                AddLabel(new Vector2(valueLeftMargin, topMargin), item.Stats.CriticalChance.ToString() + " (" + item.Stats.CriticalDamage.ToString() + ")", normalFontSize, Color.white, TextAnchor.UpperLeft);
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
        topMargin += 10;
        AddLabel(new Vector2(0, topMargin), "Requirements", 16, Color.gray, TextAnchor.UpperCenter);
        topMargin += 20;


        if (item.RequiredLevel != 0)
        {
            if (CurrentGame.Instance.Player.Level < item.RequiredLevel)
            {
                AddLabel(new Vector2(160, topMargin), "Level: " + item.RequiredLevel.ToString(), 20, Color.red, TextAnchor.UpperLeft);
            }
            else
            {
                AddLabel(new Vector2(160, topMargin), "Level: " + item.RequiredLevel.ToString(), 20, Color.grey, TextAnchor.UpperLeft);
            }
        }
        if (item.RequiredStats.Count != 0)
        {
            string requiredStats = "";
            for (int i = 0; i < item.RequiredStats.Count; i++)
            {
                requiredStats += item.RequiredStats[i].Current + " " + AttributeHelper.GetEnumDescription(item.RequiredStats[i].Type);
                if (i < item.RequiredStats.Count - 1)
                {
                    requiredStats += ", ";
                }
            }
            //To change right margin set with less then 204
            AddLabel(new Vector2(-160, topMargin), requiredStats, 20, Color.grey, TextAnchor.UpperRight);
        }

        topMargin += 28;

        _currentHeight = topMargin;
    }

    private void SetMagicAttributes(InventoryQuest.Components.Items.Item item)
    {
        switch (item.ValidSlot)
        {
            case EnumItemSlot.Head:
            case EnumItemSlot.Chest:
            case EnumItemSlot.Waist:
            case EnumItemSlot.Legs:
            case EnumItemSlot.Feet:
            case EnumItemSlot.Shoulders:
            case EnumItemSlot.Hands:
            case EnumItemSlot.Back:
                SetMagicDescription(new List<EnumStatItemPartType>()
                    {
                        EnumStatItemPartType.BaseType,
                        EnumStatItemPartType.CharacterType,
                        EnumStatItemPartType.ShieldType,
                        EnumStatItemPartType.WeaponType,
                    }, item);
                break;
            case EnumItemSlot.OffHand:
                SetMagicDescription(new List<EnumStatItemPartType>()
                    {
                        EnumStatItemPartType.BaseType,
                        EnumStatItemPartType.CharacterType,
                        EnumStatItemPartType.ArmorType,
                        EnumStatItemPartType.WeaponType,
                    }, item);
                break;
            case EnumItemSlot.Weapon:
                SetMagicDescription(new List<EnumStatItemPartType>()
                    {
                        EnumStatItemPartType.ArmorType,
                        EnumStatItemPartType.BaseType,
                        EnumStatItemPartType.CharacterType,
                        EnumStatItemPartType.ShieldType,
                    }, item);
                break;
        }
    }

    private void SetMagicDescription(List<EnumStatItemPartType> types, InventoryQuest.Components.Items.Item item)
    {
        int topMargin = _currentHeight;
        int counter = 0;

        int magicAttributeLabelHeight = 24;
        float magicAttributeLabelMargin = 160;
        //topMargin += 0;

        //movement in x direction
        foreach (var stat in item.Stats.GetAllStatsInt())
        {
            if (stat.Current != 0)
            {
                bool addNewLabel = false;
                foreach (var type in types)
                {
                    if (stat.Type.GetAttributeOfType<StatTypeAttribute>().Type == type)
                    {
                        addNewLabel = true;
                    }
                }

                if (addNewLabel)
                {
                    topMargin += magicAttributeLabelHeight;
                    AddLabel(
                        new Vector2(magicAttributeLabelMargin, topMargin),
                        "+" + stat.Current + " " + TypeStatsUtils.GetNameAttribute((int)stat.Type + 1).LongName,
                        20,
                        CurrentRarityColor,
                        TextAnchor.UpperLeft
                    );
                    counter++;
                }
            }
            if (stat.Extend != stat.Base)
            {
                topMargin += magicAttributeLabelHeight;
                AddLabel(
                    new Vector2(magicAttributeLabelMargin, topMargin),
                    "+" + (stat.Extend - stat.Base) + " " + TypeStatsUtils.GetNameAttribute((int)stat.Type + 1).LongName,
                    20,
                    CurrentRarityColor,
                    TextAnchor.UpperLeft
                );
                counter++;
            }
        }

        foreach (var stat in item.Stats.GetAllStatsFloat())
        {
            if (stat.Current != 0)
            {
                bool addNewLabel = false;
                foreach (var type in types)
                {
                    if (AttributeHelper.GetAttributeOfType<StatTypeAttribute>(stat.Type).Type == type)
                    {
                        addNewLabel = true;
                    }
                }

                if (addNewLabel)
                {
                    topMargin += magicAttributeLabelHeight;
                    AddLabel(
                        new Vector2(magicAttributeLabelMargin, topMargin),
                        "+" + stat.Current.ToString("0.##") + " " + TypeStatsUtils.GetNameAttribute((int)stat.Type + 1).LongName,
                        20,
                        CurrentRarityColor,
                        TextAnchor.UpperLeft
                    );
                    counter++;
                }
            }
            if (stat.Extend != stat.Base)
            {
                topMargin += magicAttributeLabelHeight;
                AddLabel(
                    new Vector2(magicAttributeLabelMargin, topMargin),
                    "+" + (stat.Extend - stat.Base).ToString("0.##") + " " + TypeStatsUtils.GetNameAttribute((int)stat.Type + 1).LongName,
                    20,
                    CurrentRarityColor,
                    TextAnchor.UpperLeft
                );
                counter++;
            }
        }

        if (counter > 0)
        {
            topMargin += 24;
            AddSeparator(new Vector2(0, _currentHeight));
            _currentHeight = topMargin;
        }


    }

    void CreateFlavor(InventoryQuest.Components.Items.Item item)
    {
        // The window often does not display correctly when this if case does not fire
        // I have no idea why.

        if (item.FlavorText != null && item.FlavorText != String.Empty)
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

    //__________________________________________Static methods_____________________________________________

    /// <summary>
    /// If item should be removed and there is posibility of reference beeing set up on ToolTipManager call this method
    /// </summary>
    public void RemoveTooltipReference()
    {
        Show = true;
        _item = null;
    }

}
