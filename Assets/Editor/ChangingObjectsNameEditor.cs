using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;

public class ChangingObjectsNameEditor : EditorWindow
{
    [MenuItem("Window/Apply modified names")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ChangingObjectsNameEditor window = (ChangingObjectsNameEditor)EditorWindow.GetWindow(typeof(ChangingObjectsNameEditor));
        window.Show();
    }


    public void OnGUI()
    {
        if (GUILayout.Button("Apply modified names"))
        {
            foreach (GameObject item in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (!item.GetComponents<MonoBehaviour>().Any())
                {
                    if (item.name[0] != '[')
                    {
                        item.name = "[" + item.name + "]";
                    }
                }
                else
                {
                    if (item.name[0] == '[')
                    {
                        item.name = item.name.Replace("[", "");
                        item.name = item.name.Replace("]", "");
                    }
                }
            }
        }
    }
}
