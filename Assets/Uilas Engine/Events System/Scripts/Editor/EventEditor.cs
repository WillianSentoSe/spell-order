using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Event))]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Event ev = (Event)target;

        ev.fixedDuration = EditorGUILayout.Toggle("Fixed Duration", ev.fixedDuration);

        ev.startDelay = EditorGUILayout.FloatField("Start Delay", ev.startDelay);

        if (ev.fixedDuration)
        {
            ev.duration = EditorGUILayout.FloatField("Duration", ev.duration);
        }

        EditorGUILayout.Separator();

        OnEventGUI();
    }

    public virtual void OnEventGUI()
    {

    }
}
