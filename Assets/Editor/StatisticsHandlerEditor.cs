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
        EditorGUILayout.HelpBox("Za dużo zachodu w dodawaniu inspektora więc idę na łatwiznę. If bool is true only \"stat\" and \"value\" will work, else only \"skill\"", MessageType.Info);
        DrawDefaultInspector();
    }
}
