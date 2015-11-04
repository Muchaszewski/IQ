using UnityEngine;
using System.Collections;
using Extensions;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine.Events;

[CanEditMultipleObjects]
[CustomEditor(typeof(ExtendedButton), true)]
public class ExtendedButtonEditor : ButtonEditor
{
    private ExtendedButton _script;
    private SerializedProperty _hasAudioClips;
    private SerializedProperty _audioClipHoover;
    private SerializedProperty _audioClipPress;
    private SerializedProperty _audioClipRelase;

    private AnimBool m_ShowSoundTilt = new AnimBool();


    protected override void OnEnable()
    {
        base.OnEnable();
        _script = (ExtendedButton)target;
        this._hasAudioClips = base.serializedObject.FindProperty("_hasAudioClips");
        this._audioClipHoover = base.serializedObject.FindProperty("_audioClipHoover");
        this._audioClipPress = base.serializedObject.FindProperty("_audioClipPress");
        this._audioClipRelase = base.serializedObject.FindProperty("_audioClipRelase");

        this.m_ShowSoundTilt.value = _hasAudioClips.boolValue;

        this.m_ShowSoundTilt.valueChanged.AddListener(new UnityAction(base.Repaint));

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.m_ShowSoundTilt.valueChanged.RemoveListener(new UnityAction(base.Repaint));
    }

    /// <summary>
    ///   <para>See [[Editor.OnInspectorGUI]].</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();

        EditorGUILayout.PropertyField(_hasAudioClips);
        this.m_ShowSoundTilt.target = _hasAudioClips.boolValue;
        if (EditorGUILayout.BeginFadeGroup(m_ShowSoundTilt.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_audioClipHoover);
            EditorGUILayout.PropertyField(_audioClipPress);
            EditorGUILayout.PropertyField(_audioClipRelase);

            if (_audioClipHoover.serializedObject != null)
            {

            }
            if (_script.GetComponent<AudioSource>() == null)
            {
                GUILayout.Space(3f);
                ExtendedGUI.UIIndent(() =>
                {
                    if (GUILayout.Button("Add audio source"))
                    {
                        Undo.AddComponent<AudioSource>(_script.gameObject);
                    }
                }, 15);
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
