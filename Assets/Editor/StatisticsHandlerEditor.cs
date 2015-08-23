using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using UnityEditor;

[CustomEditor(typeof(StatisticHandler))]
[CanEditMultipleObjects]
[DisallowMultipleComponent]
public class StatisticsHandlerEditor : Editor
{

    private SerializedProperty skillProperty;
    private SerializedProperty statProperty;
    private SerializedProperty valueProperty;


    private StatisticHandler script;
    void OnEnable()
    {
        script = (StatisticHandler)target;
    }

    void FindProperties()
    {
        skillProperty = serializedObject.FindProperty("skill");
        statProperty = serializedObject.FindProperty("stat");
        valueProperty = serializedObject.FindProperty("value");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("All of the below will applay as player stat", MessageType.Info);
        DrawDefaultInspector();
        GUILayout.Space(5);
        if (script.statType == EnumStatisticHandler.Stat)
        {
            script.stat = (EnumTypeStat)EditorGUILayout.EnumPopup("Stat to update", script.stat);
            script.value = (EnumStatValue)EditorGUILayout.EnumPopup("Stat state", script.value);
        }
        else if (script.statType == EnumStatisticHandler.Skill)
        {
            script.skill = (EnumItemClassSkill)EditorGUILayout.EnumPopup("Skill to update", script.skill);
            script.level = EditorGUILayout.Toggle("Show as Level", script.level);
        }
        else if (script.statType == EnumStatisticHandler.Entity)
        {
            script.entityStatType = (EnumPlayerBasics)EditorGUILayout.EnumPopup("Basic type", script.entityStatType);
        }
        else if (script.statType == EnumStatisticHandler.Special)
        {
            script.statName = EditorGUILayout.TextField("Custom name", script.statName);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Set this value as updetable"))
        {
            script.gameObject.AddComponent<StatisticHandlerUpdater>();
        }
    }
}
