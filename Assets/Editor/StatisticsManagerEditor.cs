using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(StatisticsManager))]
public class StatisticsManagerEditor : Editor
{
    private StringBuilder codeString = new StringBuilder();

    private StatisticsManager script;
    void OnEnable()
    {
        script = (StatisticsManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Create string from labels"))
        {
            codeString = GetAllChildrens();
        }
        if (!String.IsNullOrEmpty(codeString.ToString()))
        {
            GUILayout.TextArea(codeString.ToString());
            EditorGUILayout.HelpBox("To apply changes to live code, copy above code and paste it to StatisticsManager class", MessageType.Info);
            EditorGUILayout.HelpBox("Note that it wont change anything since StatisticHandler is not implemented yet", MessageType.Warning);

        }
        //        if (GUILayout.Button("Set preview values"))
        //        {
        //            ClearTooltipEditor();
        //#pragma warning disable 618
        //            script.CreateLabels();
        //#pragma warning restore 618
        //            Debug.Log("Updated");
        //        }
    }

    void ClearTooltipEditor()
    {
        script.StatisticsTexts = new List<StatisticsManager.TextObjectPair>();
        var children = new List<GameObject>();
        children.ForEach(child => Destroy(child));
        foreach (Transform child in script.transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(DestroyImmediate);

    }

    StringBuilder GetAllChildrens()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < script.transform.childCount; i++)
        {
            var text = script.transform.GetChild(i).GetComponent<Text>();
            if (text != null)
            {
                sb.AppendLine("AddLabel(" +
                              "new Vector2(" + text.GetComponent<RectTransform>().anchoredPosition.x + "," + text.GetComponent<RectTransform>().anchoredPosition.y + "), " +
                              text.text + ", " +
                              text.fontSize + ", " +
                              "new Color(" + text.color.r + "," + text.color.g + "," + text.color.b + "," + text.color.a + "), " +
                              "TextAnchor."+text.alignment + ");"
                              );
            }
        }
        return sb;
    }
}
