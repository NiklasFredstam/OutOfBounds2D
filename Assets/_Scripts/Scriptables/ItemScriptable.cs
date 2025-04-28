using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Item")]
public class ItemScriptable : ScriptableObject
{
    public string ItemName;
    public Sprite WindowSprite;
    public ItemType ItemType;
    public ItemStats ItemStats;
    public List<AbilityType> Abilities;

    public Item CreateItemInstance()
    {
        Item item = ItemType switch
        {
            ItemType.TheBomb => new BombItem(),
            _ => null
        };

        if (item == null)
        {
            Debug.LogWarning($"Ability type {ItemType} is not supported.");
            return null;
        }

        item.Initialize(this);
        return item;
    }
}

public enum ItemType
{
    TheBomb
}

