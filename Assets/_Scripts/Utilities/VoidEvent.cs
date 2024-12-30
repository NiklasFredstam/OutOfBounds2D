using System;
using UnityEngine;

public class VoidEvent
{
    private Action _event;
    public virtual void Subscribe(Action subscriber)
    {
        _event += subscriber;
        Log($"Subscribed: {subscriber.Method.Name}");
    }

    public virtual void Unsubscribe(Action subscriber)
    {
        _event -= subscriber;
        Log($"Unsubscribed: {subscriber.Method.Name}");
    }

    public void Invoke()
    {
        if (_event == null)
        {
            Log("No subscribers to invoke.");
            return;
        }

        Log("Invoking event...");
        _event.Invoke();
    }

    protected void Log(string message)
    {
        Debug.Log($"[DebuggableEvent] {message}");
    }
}