using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;

public class DebugOptions : MonoBehaviour
{
    private int _equipmentSlider = 1;
    private EnumItemRarity _rarity = EnumItemRarity.Mythical;
    //private EnumItemRarity _matchRarity = EnumItemRarity.Poor;
    //private EnumItemType _enumItemType;
    //private string _name = "Put item name here";
    //private int _level = 1;

    public void OnGUI()
    {
        var player = CurrentGame.Instance.Player;
        var canvas = FindObjectOfType<Canvas>();
        var size = GetComponent<RectTransform>().sizeDelta;
        GUILayout.BeginArea(new Rect(new Vector2(transform.position.x - (size.x * canvas.transform.localScale.x + 10) / 2f, transform.position.y - (size.y * canvas.transform.localScale.y) / 2f), size * canvas.transform.localScale.x));
        GUILayout.Label("Debug Options");
        GUILayout.Space(10);
        GUILayout.Label("Player statistics");
        GUIButtonLabel("Level up", "Level: " + player.Level, () => player.Level++);
        GUIButtonLabel("Add 1000 experienec", "Experience: " + player.Experience, () => player.Experience += 1000);
        GUILayout.Space(10);
        GUILayout.Label("Equipment operations");

        //ADD ITEMS ADV
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add random item from current spot"))
        {
            for (int i = 0; i < _equipmentSlider; i++)
            {
                player.Inventory.AddItem(
                    RandomItemFactory.CreateItem(CurrentGame.Instance.Spot, _rarity));
            }
        }
        GUILayout.Label("Items count: " + player.Inventory.Items.Count);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        //_rarity = (EnumItemRarity)GUILayout.EnumPopup(_rarity);
        _equipmentSlider = (int)GUILayout.HorizontalSlider(_equipmentSlider, 1, 100);
        GUILayout.Label(_equipmentSlider.ToString());
        GUILayout.EndHorizontal();

        //GUILayout.Space(3);

        //GUILayout.BeginHorizontal();
        //_name = GUILayout.TextField(_name);
        //int.TryParse(GUILayout.TextField(_level.ToString()), out _level);
        //_rarity = (EnumItemRarity)EditorGUILayout.EnumPopup(_rarity);
        //_matchRarity = (EnumItemRarity)EditorGUILayout.EnumPopup(_matchRarity);
        //GUILayout.EndHorizontal();
        //GUILayout.BeginHorizontal();
        //_enumItemType = (EnumItemType)EditorGUILayout.EnumPopup(_enumItemType);
        //if (GUILayout.Button("ADV ITEM CREATION"))
        //{
        //    var list = RandomItemFactory.CreateCustomItems(_level, _matchRarity, _rarity, _enumItemType, _name);
        //    if (list != null)
        //    {
        //        foreach (var item in list)
        //        {
        //            player.Inventory.AddItem(item);
        //        }
        //    }
        //}
        //GUILayout.EndHorizontal();
        //ADD ITEMS ADV

        if (GUILayout.Button("Finish Area"))
        {
            CurrentGame.Instance.Spot.Progress = CurrentGame.Instance.Spot.MonsterValueToCompleteArea;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Inventory"))
        {
            CurrentGame.Instance.Player.Inventory.Items = new SortedList<int, Item>();
            InventoryPanel.Instance.PopulateInventory();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    void GUIButtonLabel(string buttonName, string labelContent, Action action)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(buttonName))
        {
            action.Invoke();
        }
        GUILayout.Label(labelContent);
        GUILayout.EndHorizontal();
    }
}
