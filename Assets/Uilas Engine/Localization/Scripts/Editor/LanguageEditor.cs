using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Language))]
public class LanguageEditor : Editor
{
    private const int spacingSize = 20;

    private string newValue;
    private string newKey;
    private string searchKey = "";
    private List<string> searchResult;

    private Language language;

    public override void OnInspectorGUI()
    {
        language = (Language)target;

        DisplayDefaultLanguageLabel();

        base.OnInspectorGUI();

        DisplayNewElementMenu();

        DisplaySearchMenu();

        DisplayActionButtons();

        string elementsInfo = "Total: " + language.Text.Count();
        EditorGUILayout.LabelField(elementsInfo);
    }

    private void DisplayActionButtons()
    {
        EditorGUILayout.Space(spacingSize);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            language.SaveToJson();
        }

        if (GUILayout.Button("Load"))
        {
            language.LoadFromJson();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DisplayElement(string _key)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);

        GUILayout.Label(_key);

        if (GUILayout.Button("x", GUILayout.MaxWidth(20)))
        {
            language.RemoveElement(_key);
            searchResult.Remove(_key);
            return;
        }
        EditorGUILayout.EndHorizontal();

        language.Text.SetText(_key, GUILayout.TextArea(language.Text.GetText(_key)));

        EditorGUILayout.EndVertical();
    }

    private void DisplayNewElementMenu()
    {
        EditorGUILayout.Space(spacingSize);

        // New element fields
        EditorGUILayout.LabelField("New Element");

        EditorGUILayout.BeginVertical(GUI.skin.box);

        newKey = EditorGUILayout.TextField(newKey);
        newValue = EditorGUILayout.TextArea(newValue, GUILayout.MinHeight(60));

        // New element actions
        if (GUILayout.Button("Add Element"))
        {
            language.AddElement(newKey, newValue);

            GUI.FocusControl("");

            newKey = "";
            newValue = "";
        }

        EditorGUILayout.EndVertical();
    }

    private void DisplaySearchMenu()
    {
        EditorGUILayout.Space(spacingSize);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);

        searchKey = EditorGUILayout.TextField(searchKey);

        // New element actions
        if (GUILayout.Button("Find", GUILayout.MaxWidth(100)))
        {
            if (searchResult != null)
            {
                searchResult.Clear();
            }

            searchResult = language.SearchByKey(searchKey);
        }
        
        EditorGUILayout.EndHorizontal();

        if (searchResult != null)
        {
            string elementsInfo = "Found " + searchResult.Count + " elements.";
            EditorGUILayout.LabelField(elementsInfo);

            for (int i = searchResult.Count - 1; i >= 0; i--)
            {
                DisplayElement(searchResult[i]);
            }
        }
    }

    private void DisplayDefaultLanguageLabel()
    {
        if (Language.current != language)
        {
            if (GUILayout.Button("Set Default", GUILayout.Width(100)))
            {
                Language.SetLanguage(language);
            }
        }
    }
}
