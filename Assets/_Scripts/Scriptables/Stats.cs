using System;


[Serializable]
public struct CharacterStats
{
    public int Example;
}

[Serializable]
public struct UnitStats
{
    public bool Alive;
    public int Speed;
    public int Power;
    public int DestructivePower;
    public int DamageDealt;
}

[Serializable]
public struct MoveableStats
{
    public int Weight;
    public bool IsOnGrid;
}

[Serializable]
public struct TileStats
{
    public int BaseHealth;
    public int CurrentHealth;
    public int MovementReduction;
}


//todo, use this instead for abilities?
[Serializable]
public struct AbilityStats
{
    public int Range;
}

public struct ItemStats
{
    public int Weight;
}

