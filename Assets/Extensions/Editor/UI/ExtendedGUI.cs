using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public static class ExtendedGUI 
{
    public static void UIIndent(Action render, int tabSize = 4)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * tabSize);
        render();
        GUILayout.EndHorizontal();
    }
}
