using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryPanel : MonoBehaviour {

    [SerializeField]
    [HideInInspector]
    private List<Item> ItemsList = new List<Item>();

    public void AddToPanel(Item item)
    {
        item.transform.SetParent(Item.Inventory.transform);
        item.RectTransform.anchoredPosition = Vector2.one;
    }

    void ResolvePosition()
    {
        
    }

    public void RemoveFromPanel(Item item)
    {
        item.transform.SetParent(Item.Canvas.transform);
    }

}
