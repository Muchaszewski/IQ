using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MovableAnimator))]
public class MovableAnimatorEditor : Editor
{
    private Vector2 draggedPosition;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Sorry but every value have to be set manualy\r\nCustom inspector is too complicated to implement", MessageType.Info);
        DrawDefaultInspector();
        DropAreaGUI();
        EditorGUILayout.LabelField(draggedPosition.ToString());
    }

    public void DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Drop GameObject with RectTransform to get position");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        draggedPosition = ((GameObject)dragged_object).GetComponent<RectTransform>().anchoredPosition;
                    }
                }
                break;
        }
    }
}
