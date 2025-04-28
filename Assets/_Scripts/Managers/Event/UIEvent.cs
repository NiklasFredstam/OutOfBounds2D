using System;
using Unity.VisualScripting;

public class UIEvent<T>
{
    public string Name { get; }
    public Action<T> OnUIEvent { get; set; }

    public UIEvent(string name)
    {
        Name = name;
    }

    public void Invoke(T arg)
    {
        OnUIEvent?.Invoke(arg);
    }
    public void Clear()
    {
        OnUIEvent = null;
    }

    public void SubscribeOn(Action<T> method)
    {
        OnUIEvent += method;
    }

    public void UnsubscribeOn(Action<T> method)
    {
        OnUIEvent -= method;
    }


}