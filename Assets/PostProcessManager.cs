using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PostProcessManager : MonoBehaviour
{
    public Volume _volume;
    private Vignette _vignette;
    private FilmGrain _filmGrain;
    public float vignetteEndIntensity;
    public float filmGrainEndIntensity;

    private void Start()
    {
        _volume.GetComponent<Volume>();
        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _filmGrain);
        _vignette.intensity.value = 0;
        _filmGrain.intensity.value = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            _vignette.intensity.value = 0.5f;
            _filmGrain.intensity.value = 0.8f;
        }
    }

    public IEnumerator SunshinePostProcess(float lerpDuration)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            _vignette.intensity.value = Mathf.Lerp(0, vignetteEndIntensity, timeElapsed / lerpDuration);
            _filmGrain.intensity.value = Mathf.Lerp(0, filmGrainEndIntensity, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _vignette.intensity.value = vignetteEndIntensity;
        _filmGrain.intensity.value = filmGrainEndIntensity;
    }
    
}
