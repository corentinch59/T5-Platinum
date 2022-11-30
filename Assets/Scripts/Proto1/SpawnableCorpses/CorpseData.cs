using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CorpseData
{
    [Header("Name")]              public string name;
    [Header("Size")]              public RequestDataBase.size size;
    [Header("Corpse Type")]       public RequestDataBase.corpseType corpseType;
    [Header("Localisations")]     public RequestDataBase.localisation localisation;
    [Header("Coffin Type")]       public RequestDataBase.coffin coffinType;
}
