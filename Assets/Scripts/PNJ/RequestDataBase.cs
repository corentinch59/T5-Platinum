using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequestDataBase 
{
    public string corpseName;
    public size siz;
    public localisation loc;
    public corpseType corps;

    public enum localisation
    {
        NONE,
        TREE,
        WATER,
        FLOWER,
        SHRINE,
    }


    public enum corpseType
    {
        NONE,
        CORGNON1 = 1,
        CORGNON2 = 2,
        CORGNON3 = 3,
        CORGNON4 = 4,
        CORGNON5 = 5,
        CORGNON6 = 6,
        CORGNON7 = 7,
        CORGNON8 = 8,
        LEZARD1 = 5,
        LEZARD2 = 6,
        LEZARD3 = 7,
        LEZARD4 = 8,
        
    }
    

    public enum size
    {
        SMALL,
        MEDIUM,
    }
}
