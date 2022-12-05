using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayStars : MonoBehaviour
{
    [SerializeField] private Slider satisfactionSlider;
    private List<GameObject> stars = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            stars.Add(transform.GetChild(i).gameObject);

            transform.GetChild(i).gameObject.SetActive(false);
            Debug.Log("Stars");
        }

        DisplayStars();
    }

    private void DisplayStars()
    {
        Debug.Log("Voir avec les GD pour comment on affiche le score");
    }
}
