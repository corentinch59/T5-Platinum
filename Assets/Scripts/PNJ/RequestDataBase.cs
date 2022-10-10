using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequestDataBase 
{
        public string name;
        public localisation loc;
        public coffin cof;
        public corpseType corp;
    
        
        public enum localisation
        {
                TREE,
                WATER,
                INCINERATOR,
        }
        public enum coffin
        {
                COFFIN,
                JARRE,
        }
        public enum corpseType
        {
                MONSTERVERT,
                MONSTREBLEU,
        } 

}
