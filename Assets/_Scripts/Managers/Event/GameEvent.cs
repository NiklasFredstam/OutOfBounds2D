using System;

public class GameEvent<T>
{
    public string Name { get; }
    public Action<T> BeforeAction { get; set; }
    public Action<T> OnAction { get; set; }
    public Action<T> AfterAction { get; set; }

    public GameEvent(string name)
    {
        Name = name;
    }

    public void Invoke(T arg)
    {
        BeforeAction?.Invoke(arg);
        OnAction?.Invoke(arg);
        AfterAction?.Invoke(arg);
    }

    public void SubscribeBefore(Action<T> method)
    {
        BeforeAction += method;
    }

    public void SubscribeOn(Action<T> method)
    {
        OnAction += method;
    }

    public void SubscribeAfter(Action<T> method)
    {
        AfterAction += method;
    }

    public void UnsubscribeBefore(Action<T> method)
    {
        BeforeAction -= method;
    }

    public void UnsubscribeOn(Action<T> method)
    {
        OnAction -= method;
    }

    public void UnsubscribeAfter(Action<T> method)
    {
        BeforeAction -= method;
    }

    public void Clear()
    {
        BeforeAction = null;
        OnAction = null;
        AfterAction = null;
    }

}