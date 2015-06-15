using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StatisticHandler))]
public class StatisticsHandlerEditor : Editor {

    private StatisticHandler script;
    void OnEnable()
    {
        script = (StatisticHandler)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
