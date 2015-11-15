using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using Extensions;

public class ExtendedUIWindow : EditorWindow
{
    private static bool _foldoutUI;

    [MenuItem("Extensions/Extend GameObjects")]
    static void Init()
    {
        EditorWindow.GetWindow<ExtendedUIWindow>().Show();
    }

    void OnGUI()
    {
        _foldoutUI = EditorGUILayout.Foldout(_foldoutUI, "UI");
        if (_foldoutUI)
        {
            EditorGUI.indentLevel++;

            GUI.enabled = (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Button>() != null);
            if (GUILayout.Button("Convert Selected Button to Extended verison"))
            {
                var go = new GameObject();
                var extendedButton = go.AddComponent<ExtendedButton>();
                var buttonGo = Selection.activeGameObject.gameObject;
                extendedButton = extendedButton.ConvertFrom(Selection.activeGameObject.GetComponent<Button>());
                DestroyImmediate(Selection.activeGameObject.GetComponent<Button>());
                buttonGo.AddComponent<ExtendedButton>(extendedButton);
                DestroyImmediate(go);
            }
            GUI.enabled = true;
            if (GUILayout.Button("Convert All Buttons to Extended version"))
            {
                foreach (var button in FindObjectsOfType<Button>())
                {
                    //Some magic with empty object
                    //TODO performance impvorment
                    var go = new GameObject();
                    var extendedButton = go.AddComponent<ExtendedButton>();
                    var buttonGo = button.gameObject;
                    extendedButton = extendedButton.ConvertFrom(button);
                    DestroyImmediate(button);
                    buttonGo.AddComponent<ExtendedButton>(extendedButton);
                    DestroyImmediate(go);
                }
            }

            EditorGUI.indentLevel--;
        }
    }
}
