using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

[CustomEditor(typeof(GetStat))]
[CanEditMultipleObjects]
public class GetStatEditor : Editor
{
    private GetStat script;
    void OnEnable()
    {
        script = (GetStat)target;
    }

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        if (GUILayout.Button("Set preview values"))
        {
            script.GetComponent<Text>().text = script.SetPreview();
        }
    }
}
