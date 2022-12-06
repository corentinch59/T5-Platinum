using UnityEngine;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound[] sounds;

    [SerializeField] private AudioMixerGroup MusicGroup;
    [SerializeField] private AudioMixerGroup SFXGroup;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);


        for (int i = 0; i < sounds.Length; i++)
        {
            Sound s = sounds[i];

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
            if (s.type == SoundType.MUSIC)
            {
                s.source.outputAudioMixerGroup = MusicGroup;
            }
            else
            {
                s.source.outputAudioMixerGroup = SFXGroup;
            }
        }
    }

    public Sound Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return null;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
        //Debug.Log("Played :" + " " + s.name);
        return s;

        // check if playing for drag or bug
        // vvvvv
        /*if (!s.source.isPlaying)
        {
            s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
            s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

            s.source.Play();
            //Debug.Log("Played :" + " " + s.name);
            return s;
        }
        return null;*/
    }

    public Sound Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return null;
        }

        s.source.Stop();
        //Debug.Log("Stoped :" + " " + s.name);
        return s;
    }

    public Sound GetSound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return null;
        }
        return s;
    }
}
public enum SoundType{
    NONE,
    MUSIC,
    SFX
}
