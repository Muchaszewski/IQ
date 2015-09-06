using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;
using UnityEditor;

public class DebugOptions : MonoBehaviour
{
    private int _equipmentSlider = 1;
    private EnumItemRarity _rarity = EnumItemRarity.Mythical;

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
        if (GUILayout.Button("Add random item"))
        {
            for (int i = 0; i < _equipmentSlider; i++)
            {
                player.Inventory.AddItem(
                    RandomItemFactory.CreateItem(player.Level, CurrentGame.Instance.Spot, _rarity));
            }
        }
        GUILayout.Label("Items count: " + player.Inventory.Items.Count);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        _rarity = (EnumItemRarity)EditorGUILayout.EnumPopup(_rarity);
        _equipmentSlider = (int)GUILayout.HorizontalSlider(_equipmentSlider, 1, 100);
        GUILayout.Label(_equipmentSlider.ToString());
        GUILayout.EndHorizontal();
        //ADD ITEMS ADV

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
