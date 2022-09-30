using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RequestsData")]
public class ScriptableRequest :ScriptableObject
{
     public List<RequestDataBase> _dataBase;
}
