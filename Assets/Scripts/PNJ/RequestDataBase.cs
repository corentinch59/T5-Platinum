using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequestDataBase 
{
    public string name;
    public size siz;
    public localisation loc;
    public coffin cof;
    public corpseType corps;
    public specificity spec;

    public enum localisation
    {
        TREE,
        WATER,
        INCINERATOR,
    }

    public enum coffin
    {
        CHEAP,
        CLASSIC,
        LUXURY,
        JAR,
    }

    public enum corpseType
    {
        MONSTERVERT,
        MONSTREBLEU,
    }

    public enum specificity
    {
        HAT,
        GLASSES,
        EYEPATCH,
    }

    public enum size
    {
        SMALL,
        MEDIUM,
        LARGE,
    }
}
