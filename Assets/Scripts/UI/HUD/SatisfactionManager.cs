using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionManager : MonoBehaviour
{
    private Slider satisfactionSlider = default;

    private void Start()
    {
        satisfactionSlider = GetComponent<Slider>();


        Quest.onFinishQuest += AddSatisfaction;
    }


    private void AddSatisfaction(int value)
    {
        satisfactionSlider.value += value;
    }

    private void OnDestroy()
    {
        Quest.onFinishQuest -= AddSatisfaction;
    }
}
