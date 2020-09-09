using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;

[CustomEditor(typeof(EventChain))]
public class EventChainEditor : Editor
{
    protected EventChain chain;
    private Events selectedEvent;

    private GUIStyle guiStyleBox;
    private GUIStyle guiStyleLabel;
    private GUIStyle guiStyleButton;

    public void OnEnable()
    {
        chain = (EventChain)target;
    }

    public override void OnInspectorGUI()
    {
        // Setting the GUI Styles
        guiStyleBox = new GUIStyle(GUI.skin.box);
        guiStyleBox.margin = new RectOffset(0, 20, 10, 0);
        guiStyleBox.padding = new RectOffset(20, 20, 10, 10);

        guiStyleLabel = new GUIStyle(GUI.skin.label);
        guiStyleLabel.richText = true;

        guiStyleButton = new GUIStyle(GUI.skin.button);
        guiStyleButton.margin = new RectOffset(0, 20, 10, 0);
        guiStyleButton.padding = new RectOffset(10, 10, 5, 5);


        // Draw chain's properties
        EditorGUILayout.Space(10);

        chain.trigger = (EventChain.EventTrigger)EditorGUILayout.EnumPopup("Trigger", chain.trigger);
        chain.radius = EditorGUILayout.FloatField("Radius", chain.radius);
        if (chain.trigger != EventChain.EventTrigger.Interact)
            chain.groundedOnly = EditorGUILayout.Toggle("Grounded Only", chain.groundedOnly);

        EditorGUILayout.Space(10);

        // For every event in the chain
        for (int i = 0; i < chain.events.Count; i++)
        {
            EditorGUILayout.BeginVertical(guiStyleBox);

            // Draw Event header
            EditorGUILayout.BeginHorizontal(guiStyleLabel);

            string label = (i + 1) + " - " + chain.events[i].GetEventName();
            //if (GUILayout.Button(chain.events[i].visible? "<b>" + label + "</b>" : label, guiStyleLabel))
            //    chain.events[i].visible = !chain.events[i].visible;


            chain.events[i].visible = EditorGUILayout.Foldout(chain.events[i].visible, label);
            EditorGUILayout.EndHorizontal();

            // Draw Event content
            if (chain.events[i].visible)
            {

                // Call the expecific event's editor
                EditorGUILayout.BeginVertical();

                CreateEditor(chain.events[i]).OnInspectorGUI();

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(10);

                // Event actions
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Move Up"))
                {
                    chain.MoveEventUp(i);
                }

                if (GUILayout.Button("Move Down"))
                {
                    chain.MoveEventDown(i);
                }

                if (GUILayout.Button("Duplicate"))
                {
                    chain.DuplicateEvent(i);
                }

                if (GUILayout.Button("Delete"))
                {
                    chain.DeleteEvent(i);
                }

                EditorGUILayout.EndHorizontal();

            }

            EditorGUILayout.EndVertical();
        }

        // Draw the event type dropdown
        EditorGUILayout.Separator();
        selectedEvent = (Events)EditorGUILayout.EnumPopup("Event Type", selectedEvent);

        // Draw Add event button
        if (GUILayout.Button("Add Event", guiStyleButton))
        {
            chain.AddEvent(GetEventType());
        }

        EditorGUILayout.Space(10);
    }

    public System.Type GetEventType()
    {
        System.Type _type;

        switch (selectedEvent)
        {
            case Events.TextEvent: _type = typeof(TextEvent); break;
            case Events.QuestionEvent: _type = typeof(QuestionEvent); break;
            default: _type = typeof(Event); break;
        }

        return _type;
    }

    public enum Events
    {
        TextEvent, QuestionEvent
    }
}
