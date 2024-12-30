using System;
using UnityEngine;

public class Event<T>
{
    //If I want to send multiple things, just create complex event arg classes.
    //Make those serializable preferrably so they can be debugged more easily
    private Action<T> _event;


    //Priority should probably be a numbered Enum like:
    //damaging = 1
    //moving = 2
    //statuschange = 3
    //Or something similar. that avoids concurrency issues, 
    private int priority;


    public virtual void Subscribe(Action<T> subscriber)
    {
        _event += subscriber;
        Log($"Subscribed: {subscriber.Method.Name}");
    }

    public virtual void Unsubscribe(Action<T> subscriber)
    {
        _event -= subscriber;
        Log($"Unsubscribed: {subscriber.Method.Name}");
    }

    public void Invoke(T arg)
    {
        if (_event == null)
        {
            //Log("No subscribers to invoke.");
            return;
        }

        Log("Invoking event...");
        _event.Invoke(arg);
    }

    protected void Log(string message)
    {
        Debug.Log($"[DebuggableEvent] {message}");
    }
}
