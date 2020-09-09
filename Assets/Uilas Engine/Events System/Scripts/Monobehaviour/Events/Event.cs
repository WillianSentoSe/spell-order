using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : ScriptableObject
{
    [HideInInspector]
    public bool visible;
    public float startDelay;
    public bool fixedDuration;
    public float duration;

    public virtual string GetEventName()
    {
        return "Event";
    }
}
