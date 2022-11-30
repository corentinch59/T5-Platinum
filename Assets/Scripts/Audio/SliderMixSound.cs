using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SliderMixSound : MonoBehaviour
{

    [SerializeField] private AudioMixerGroup m_MixerGroup;
    //[SerializeField] private AudioMixer m_Mixer;
    private TextMeshProUGUI value = default;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        value = transform.Find("Value").GetComponent<TextMeshProUGUI>();
        value.text = slider.value.ToString();

        m_MixerGroup.audioMixer.SetFloat(m_MixerGroup.name + "Volume", slider.value - 80);
    }

    public void UpdateValue()
    {
        value.text = slider.value.ToString();
        m_MixerGroup.audioMixer.SetFloat(m_MixerGroup.name + "Volume", slider.value - 80);
    }
}
