using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvent : Event
{
    public TextType type;
    public Speaker speaker;
    public string text;

    public override string GetEventName()
    {
        return "Text Event";
    }

    public enum TextType
    {
        Normal, Sign, Important, None
    }

    public enum Speaker
    {
        Self, Other, None
    }
}
