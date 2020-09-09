using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "New Language", menuName = "Language")]
public class Language : ScriptableObject
{
    #region Static Properties

    public static Language current;
    public static string filePath = Path.Combine("Assets", "Language", "Json"); 

    #endregion

    #region Properties

    public string displayName;
    public string fileName;

    [SerializeField] [HideInInspector]
    private SerializableDictionary localizedText = new SerializableDictionary();

    #endregion

    #region Getters and Setters

    public SerializableDictionary Text
    {
        get
        {
            //if (texts == null)
            //{
            //    texts = new Dictionary<string, string>();
            //}

            return localizedText;
        }
    }

    #endregion

    #region Public Methods

    public void AddElement(string _key, string _value)
    {
        try
        {
            Text.Add(_key, _value);
        }
        catch (UnityException ex)
        {
            Debug.LogWarning("Failed to add element. " + ex.Message);
        }
    }

    public void RemoveElement(string _key)
    {
        try
        {
            Text.Remove(_key);
        }
        catch
        {
            Debug.LogWarning("Failed to remove from dictionary. Key (" + _key + ") not found.");
        }
    }

    public string GetText(string _key)
    {
        try
        {
            return Text.GetText(_key);
        }
        catch
        {

        }

        return "";
    }

    public List<string> SearchByKey(string _searchKey)
    {
        List<string> searchResult = new List<string>();

        string[] keys = Text.GetKeys();

        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];

            if (key == "" || key.Contains(_searchKey))
            {
                searchResult.Add(key);
            }
        }

        return searchResult;
    }

    public void LoadFromJson()
    {
        if (File.Exists(Path.Combine(filePath, fileName) + ".json"))
        {
            string json = File.ReadAllText(Path.Combine(filePath, fileName) + ".json");

            localizedText = JsonUtility.FromJson<SerializableDictionary>(json);

            Debug.Log("Json loaded from at " + Path.Combine(filePath, fileName) + ".json");
        }
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(localizedText);

        File.WriteAllText(Path.Combine(filePath, fileName) + ".json", json);

#if UNITY_EDITOR

        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("Json file saved at " + Path.Combine(filePath, fileName) + ".json");

#endif
    }

    #endregion

    #region Static Methods

    public static string Translate(string _text)
    {
        if (current == null)
        {
            throw new UnityException("First, you need to set a language. Use Language.SetLanguage().");
        }

        return current.GetText(_text);
    }

    public static void SetLanguage(Language _language)
    {
        if (current != null)
        {
            Debug.Log(current.displayName + " isn't the current language anymore.");
        }

        current = _language;
    }

    #endregion
}
