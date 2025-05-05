using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour
{
    [Serializable]
    public class EventEntry
    {
        public EventType eventType;
        public UnityEvent callback = new UnityEvent();
    }

    public enum EventType
    {
        OnTriggerEnter,
        OnTriggerExit,
        OnTriggerStay,
        OnCollisionEnter,
        OnCollisionExit,
        OnCollisionStay,
        OnTriggerEnter2D,
        OnTriggerExit2D,
        OnTriggerStay2D,
        OnCollisionEnter2D,
        OnCollisionExit2D,
        OnCollisionStay2D
    }

    [SerializeField] internal List<EventEntry> eventEntries = new List<EventEntry>();

    private void OnTriggerEnter(Collider other)
    {
        TriggerEvent(EventType.OnTriggerEnter);
    }
    private void OnTriggerExit(Collider other)
    {
        TriggerEvent(EventType.OnTriggerExit);
    }
    private void OnTriggerStay(Collider other)
    {
        TriggerEvent(EventType.OnTriggerStay);
    }
    private void OnCollisionEnter(Collision _)
    {
        TriggerEvent(EventType.OnCollisionEnter);
    }
    private void OnCollisionExit(Collision _)
    {
        TriggerEvent(EventType.OnCollisionExit);
    }
    private void OnCollisionStay(Collision _)
    {
        TriggerEvent(EventType.OnCollisionStay);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEvent(EventType.OnTriggerEnter2D);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        TriggerEvent(EventType.OnTriggerExit2D);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        TriggerEvent(EventType.OnTriggerStay2D);
    }
    private void OnCollisionEnter2D(Collision2D _)
    {
        TriggerEvent(EventType.OnCollisionEnter2D);
    }
    private void OnCollisionExit2D(Collision2D _)
    {
        TriggerEvent(EventType.OnCollisionExit2D);
    }
    private void OnCollisionStay2D(Collision2D _)
    {
        TriggerEvent(EventType.OnCollisionStay2D);
    }

    private void TriggerEvent(EventType type)
    {
        foreach (EventEntry entry in eventEntries)
        {
            if (entry.eventType == type)
            {
                entry.callback?.Invoke();
            }
        }
    }
}
