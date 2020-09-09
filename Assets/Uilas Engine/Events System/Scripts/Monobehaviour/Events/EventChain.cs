using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventChain : MonoBehaviour
{
    public List<Event> events;

    public EventTrigger trigger;
    public float radius;
    public bool groundedOnly = true;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void AddEvent(System.Type _type)
    {
        if (events == null)
            events = new List<Event>();

        Event newEvent = (Event)ScriptableObject.CreateInstance(_type);
        events.Add(newEvent);
    }

    public void DeleteEvent(int _position)
    {
        events.RemoveAt(_position);
    }

    public void MoveEventUp(int _position)
    {
        if (_position <= 0)
            return;

        Event _aux = events[_position - 1];
        events[_position - 1] = events[_position];
        events[_position] = _aux;
    }

    public void MoveEventDown(int _position)
    {
        if (_position >= events.Count - 1)
            return;

        Event _aux = events[_position + 1];
        events[_position + 1] = events[_position];
        events[_position] = _aux;
    }

    public void DuplicateEvent(int _position)
    {
        Event _newEvent = Instantiate(events[_position]) as Event;
        events.Insert(_position + 1, _newEvent);
    }

    public enum EventTrigger{
        Interact, EnterArea
    }
}
