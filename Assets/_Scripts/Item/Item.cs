using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string ItemName;
    public Sprite ItemImage;
    public ItemStats ItemStats;
    public List<Ability> Abilities;

    public virtual MoveableStats ApplyToStats(MoveableStats stats)
    {
        MoveableStats modifiedStats = stats;
        return modifiedStats;
    }
    public virtual UnitStats ApplyToStats(UnitStats stats)
    {
        UnitStats modifiedStats = stats;
        return modifiedStats;
    }
    public virtual CharacterStats ApplyToStats(CharacterStats stats)
    {
        CharacterStats modifiedStats = stats;
        return modifiedStats;
    }
    public virtual TileStats ApplyToStats(TileStats stats)
    {
        TileStats modifiedStats = stats;
        return modifiedStats;
    }


    public virtual void Initialize(ItemScriptable data)
    {
        ItemName = data.ItemName;
        ItemImage = data.WindowSprite;
        ItemStats = data.ItemStats;
        Abilities = AbilityManager.instance.CreateAbilities(data.Abilities);
    }
}