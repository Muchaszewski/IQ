using UnityEngine;
using System.Collections;
using Rotorz.ReorderableList;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(GetStatAdvance))]
public class GetStatAdvanceEditor : Editor
{

    SerializedProperty _getStats;
    private GetStatAdvance script;

    void OnEnable()
    {
        _getStats = serializedObject.FindProperty("Stats");
        script = (GetStatAdvance)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        ReorderableListGUI.Title("Multiple stats");
        ReorderableListGUI.ListField(_getStats);

        if (GUILayout.Button("Set preview values"))
        {
            script.GetComponent<Text>().text = script.SetPreview();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
