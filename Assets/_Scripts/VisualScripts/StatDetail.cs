
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class StatDetail
{
    public string StatName;
    public int Value;
    public bool IsValueHigherThanBase;
    public bool IsValueLowerThanBase;
    public DetailStatType DetailStatType;

    public StatDetail(string statName, int value, bool isValueHigher, bool isValueLower, DetailStatType detailStatType)
    {
        StatName = statName;
        Value = value;
        IsValueHigherThanBase = isValueHigher;
        IsValueLowerThanBase = isValueLower;
        DetailStatType = detailStatType;
    }
}