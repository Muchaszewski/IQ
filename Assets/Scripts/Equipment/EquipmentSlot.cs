using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using InventoryQuest;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;

public class EquipmentSlot : MonoBehaviour
{

    public static event EventHandler ItemEquiped = delegate { };
    public static event EventHandler ItemUnequiped = delegate { };


    public EnumItemSlot Slot;

    public bool FitItemBackground = true;

    public bool FitItemIcon = true;
    [HideInInspector]
    public Vector2 ItemPosition;
    public bool KeepFitted;
    [HideInInspector]
    public float ItemScale = 1;
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

    public void AddToPanel(ItemIcon itemIcon)
    {
        itemIcon.transform.SetParent(transform);
    }

    public bool IsEquipable(ItemIcon itemIcon)
    {
        var item = itemIcon.ItemData;
        var player = CurrentGame.Instance.Player;
        if (item.RequiredLevel > player.Level
            )
        {
            return false;
        }
        return true;
    }

    public void SetItemIcon(ItemIcon itemIcon)
    {
        var image = itemIcon.transform.GetChild(0).GetComponent<RectTransform>();
        //Set position
        float x = RectTransform.sizeDelta.x / 2f;
        float y = -RectTransform.sizeDelta.y / 2f;
        itemIcon.RectTransform.anchoredPosition = new Vector2(x, y);

        CurrentGame.Instance.Player.Equipment.Items[(int)Slot] = itemIcon.ItemData;
        CurrentGame.Instance.Player.Equipment.UpdateStatistics();
        ItemEquiped.Invoke(this, EventArgs.Empty);
        Debug.Log(CurrentGame.Instance.Player.Stats.Armor.Extend);
        //FitItemBackground
        if (FitItemBackground)
        {
            itemIcon.RectTransform.sizeDelta = RectTransform.sizeDelta;
            itemIcon.transform.localScale = Vector3.one;
        }

        if (FitItemIcon)
        {
            image.sizeDelta = RectTransform.sizeDelta;
        }
        else
        {
            image.anchoredPosition = ItemPosition;
        }
        if (KeepFitted)
        {
            image.transform.localScale = Vector3.one * InventoryPanel.Instance.InventoryScale;
        }
        else
        {
            image.transform.localScale = Vector3.one * ItemScale;
        }
    }

    public Rect GetAbsolutiveRect()
    {
        var canvas = GameObject.FindGameObjectWithTag("Canvas");
        return new Rect
            (
            transform.position.x,
            transform.position.y,
            RectTransform.sizeDelta.x * GameObject.FindGameObjectWithTag("Canvas").transform.localScale.x,
            RectTransform.sizeDelta.y * GameObject.FindGameObjectWithTag("Canvas").transform.localScale.y
            );
    }

    void Update()
    {
        if (CurrentGame.Instance.Player.Equipment.Items[(int)Slot] != null && transform.childCount == 0)
        {
            CurrentGame.Instance.Player.Equipment.Items[(int)Slot] = null;
            CurrentGame.Instance.Player.Equipment.UpdateStatistics();
            ItemUnequiped.Invoke(this, EventArgs.Empty);
        }
    }
}
