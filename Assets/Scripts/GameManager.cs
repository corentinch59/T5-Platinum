using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
            _instance = this;
        else
        {
            Destroy(this);
            return;
        }
    }
}