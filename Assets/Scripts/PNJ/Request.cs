using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Request : MonoBehaviour
{
    [SerializeField] private ScriptableRequest _scriptableRequest;
    [SerializeField] private ScriptableTextureData _textureData;
    [SerializeField] private RequestDataBase _requestInfos;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    

    private void Awake()
    {
        setRequest();
        
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            setRequest();
            UpdateUI();
        }
    }

    private void setRequest()
    {
        _requestInfos = _scriptableRequest._dataBase[GetRandomNumber(_scriptableRequest._dataBase.Count)];
    }

    private int GetRandomNumber(int arrayCount)
    {
        return Random.Range(0, arrayCount);
    }

    private void UpdateUI()
    {
        nameText.text = _requestInfos.name;
        TextureData tex = _textureData._TextureData;
        corpseImage.texture = tex.corpsesTex[(int)_requestInfos.corp];
        localisationImage.texture = tex.localisationTex[(int)_requestInfos.loc];
        coffinImage.texture = tex.coffinTex[(int)_requestInfos.cof];
        
        
    }
}
