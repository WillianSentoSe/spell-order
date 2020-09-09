using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEvent : TextEvent
{
    public Response[] responses;

    public override string GetEventName()
    {
        return "Question Event";
    }
}

[System.Serializable]
public class Response
{
    public string text;
    public int jumpTo;
}