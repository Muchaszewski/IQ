using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventoryQuest.Components;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Components.Items;
using InventoryQuest.Game;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ToolTipManager))]
public class TooltipManagerEditor : Editor
{
    private ToolTipManager script;
    void OnEnable()
    {
        script = (ToolTipManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Set preview values"))
        {
            var spot = CurrentGame.Instance.Spot;
            ClearTooltipEditor();
#pragma warning disable 618
            script.CreateLabels(RandomItemFactory.CreateItem(1, spot));
#pragma warning restore 618
        }
        if (GUILayout.Button("Clear tooltip"))
        {
            ClearTooltipEditor();
        }
    }


    void ClearTooltipEditor()
    {
        script.SetWindowSize(381);
        foreach (var o in script.TootipObjects)
        {
            DestroyImmediate(o);
        }
        script.TootipObjects = new List<GameObject>();
    }
}
