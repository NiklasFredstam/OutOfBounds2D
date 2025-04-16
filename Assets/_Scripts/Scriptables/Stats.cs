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
    public int Strength;
    public int Movement;
    public int DestructivePower;
    public int DamageDealt;
}

[Serializable]
public struct MoveableStats
{
    public int Weight;
}