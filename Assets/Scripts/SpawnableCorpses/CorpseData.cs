using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CorpseData
{
    [Header("Name")]              public string name;
    [Header("Texture")]           public Material texture;
    [Header("Localisations")]     public List<LocalisationsEnum> localisations;
    [Header("Coffin Type")]       public CoffinType coffinType;
    [Header("Size")] [Range(1,3)] public int size;
}
