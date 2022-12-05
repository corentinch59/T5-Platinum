using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayStars : MonoBehaviour
{

    private List<GameObject> stars = new List<GameObject>();


    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            stars.Add(transform.GetChild(i).gameObject);
        }

        
    }
}
