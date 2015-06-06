using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquipmentSlot))]
[CanEditMultipleObjects]
public class EquipmentSlotEditor : Editor
{
    private EquipmentSlot script;

    private SerializedProperty itemPositionProperty;
    private SerializedProperty itemScaleProperty;
    private SerializedProperty isScaleChangedProperty;

    void OnEnable()
    {
        script = (EquipmentSlot)target;
        itemPositionProperty = serializedObject.FindProperty("ItemPosition");
        itemScaleProperty = serializedObject.FindProperty("ItemScale");
        isScaleChangedProperty = serializedObject.FindProperty("ItemScale");

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        if (!script.FitItemIcon)
        {
            itemPositionProperty.vector2Value = EditorGUILayout.Vector2Field("Item Position", itemPositionProperty.vector2Value);
        }
        if (!script.KeepFitted)
        {
            isScaleChangedProperty.floatValue = EditorGUILayout.FloatField("Item Sacle", isScaleChangedProperty.floatValue);
        }

        script.name = "EquipmentSlot" + script.Slot.ToString();
        serializedObject.ApplyModifiedProperties();

    }

}
