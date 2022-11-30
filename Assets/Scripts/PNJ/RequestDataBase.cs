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
        //FLOWER,
        //SHRINE,
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
        CORGNON1,
        CORGNON2,
        CORGNON3,
        CORGNON4,
        LEZARD1,
        LEZARD2,
        LEZARD3,
        LEZARD4,
        
    }
    

    public enum size
    {
        SMALL,
        MEDIUM,
        LARGE,
    }
}
