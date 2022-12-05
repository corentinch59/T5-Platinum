using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FogIntensity : MonoBehaviour
{
    private Material fog;
    private Sunshine _sunshine;
    private float sunshineTime;
    public float endIntensity;
    
    void Start()
    {
        _sunshine = GameObject.FindGameObjectWithTag("Sunshine").GetComponent<Sunshine>();
        fog = gameObject.GetComponent<MeshRenderer>().materials[0];
        sunshineTime = _sunshine.timing;
        SetIntensity();
    }

    void Update()
    {
        
    }

    void SetIntensity()
    {
        fog.DOFloat(endIntensity, "_Intensity", sunshineTime);
    }
}
