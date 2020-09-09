using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextEvent))]
public class TextEventEditor : EventEditor
{
    private const int textAreaWidth = 200;

    public override void OnEventGUI()
    {
        TextEvent _textEvent = (TextEvent)target;

        _textEvent.type = (TextEvent.TextType)EditorGUILayout.EnumPopup("Type", _textEvent.type);
        _textEvent.speaker = (TextEvent.Speaker)EditorGUILayout.EnumPopup("Speaker", _textEvent.speaker);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Text", GUILayout.MinWidth(20));
        _textEvent.text = EditorGUILayout.TextArea(_textEvent.text, GUILayout.Width(textAreaWidth));
        EditorGUILayout.EndHorizontal();
    }
}
