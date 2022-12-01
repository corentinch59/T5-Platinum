using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigUpRequest : MonoBehaviour
{
    [SerializeField] private RequestDataBase requestInfo; // <- this is the quest the pnj will have
    public RequestDataBase RequestInfo { get { return requestInfo; } set { requestInfo = value; } }

    public void SetDigUpRequest()
    {
        requestInfo = QuestManager.instance.GetRequest(this);
    }
}
