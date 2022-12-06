using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager instance;

    public List<SpriteRenderer> holesSprites;
    
    
    private void Awake()
    {
        instance = this;
    }

    public void Outline()
    {
        
    }
}
