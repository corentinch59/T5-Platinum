using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequestDataBase 
{
    public string corpseName;
    public size siz;
    public localisation loc;
    public coffin cof;
    public corpseType corps;

    public enum localisation
    {
        NONE,
        TREE,
        WATER,
        //INCINERATOR,
        FLOWER,
        SHRINE,
    }

    public enum coffin
    {
        CHEAP,
        CLASSIC,
        LUXURY,
        NONE,
        JAR,
    }

    public enum corpseType
    {
        NONE,
        CORGNON1 = 1,
        CORGNON2 = 2,
        CORGNON3 = 3,
        CORGNON4 = 4,
        LEZARD1 = 5,
        LEZARD2 = 6,
        LEZARD3 = 7,
        LEZARD4 = 8,
        
    }
    

    public enum size
    {
        SMALL,
        MEDIUM,
        LARGE,
    }
}
