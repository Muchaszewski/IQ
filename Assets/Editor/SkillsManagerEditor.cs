using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SkillsManager))]
public class SkillsManagerEditor : Editor
{

    private SkillsManager script;
    void OnEnable()
    {
        script = (SkillsManager)target;
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
            Debug.Log("Updated");
        }
    }

    void ClearTooltipEditor()
    {
        script.StatisticsTexts = new List<SkillsManager.TextObjectPair>();
        var children = new List<GameObject>();
        children.ForEach(child => Destroy(child));
        foreach (Transform child in script.transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(DestroyImmediate);

    }
}
