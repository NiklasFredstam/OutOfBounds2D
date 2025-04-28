using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using UnityEngine;

public interface IInspectable
{
    List<StatDetail> GetDetails();
    string GetDescription();
    Sprite GetObjectSprite();
}