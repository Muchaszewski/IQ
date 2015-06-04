using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(StatisticsManager))]
public class StatisticsManagerEditor : Editor
{

    private StatisticsManager script;
    void OnEnable()
    {
        script = (StatisticsManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Set preview values"))
        {
            ClearTooltipEditor();
#pragma warning disable 618
            script.CreateLabels();
#pragma warning restore 618
        }
    }

    void ClearTooltipEditor()
    {
        script.StatisticsTexts = new List<StatisticsManager.TextStringPair>();
        var children = new List<GameObject>();
        children.ForEach(child => Destroy(child));
        foreach (Transform child in script.transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(DestroyImmediate);

    }
}
