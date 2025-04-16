using UnityEngine;

public interface ISelectable
{
    //Don't change the name of these methods as they are used in the InputManager
    public void OnSelect();
    public void OnDeselect();
}
