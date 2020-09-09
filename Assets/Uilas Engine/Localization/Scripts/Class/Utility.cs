using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary
{
    public SerializableDictionaryElement[] elements = new SerializableDictionaryElement[0];

    public string GetText(string _key)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].key == _key)
            {
                return elements[i].value;
            }
        }

        throw new UnityException("Key (" + _key + ") not found.");
    }

    public void SetText(string _key, string _text)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].key == _key)
            {
                elements[i].value = _text;
            }
        }
    }

    public int GetIndex(string _key)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].key == _key)
            {
                return i;
            }
        }

        throw new UnityException("Key (" + _key + ") not found.");
    }

    public void Add(string _key, string _text)
    {
        if (Contains(_key))
        {
            throw new UnityException("Key (" + _key + ") already exists.");
        }

        List<SerializableDictionaryElement> list = new List<SerializableDictionaryElement>(elements);

        list.Add(new SerializableDictionaryElement(_key, _text));

        elements = list.ToArray();
    }

    public void Remove(string _key)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].key == _key)
            {
                List<SerializableDictionaryElement> list = new List<SerializableDictionaryElement>(elements);

                list.RemoveAt(i);

                elements = list.ToArray();
            }
        }
    }

    public int Count()
    {
        return elements.Length;
    }

    public string[] GetKeys()
    {
        List<string> keyList = new List<string>();

        foreach (var element in elements)
        {
            keyList.Add(element.key);
        }

        return keyList.ToArray();
    }

    public bool Contains(string _key)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].key == _key)
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class SerializableDictionaryElement
{
    public string key;
    public string value;

    public SerializableDictionaryElement(string _key, string _value)
    {
        key = _key;
        value = _value;
    }
}