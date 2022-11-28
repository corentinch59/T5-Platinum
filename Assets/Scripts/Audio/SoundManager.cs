using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	//public AudioMixerGroup mixerGroup;

	public Sound[] sounds;


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


        //foreach (Sound s in sounds)
        //{
        //    s.source = gameObject.AddComponent<AudioSource>();
        //    s.source.clip = s.clip;
        //    s.source.loop = s.loop;
        //}


        for (int i = 0; i < sounds.Length; i++)
        {
            Sound s = sounds[i];

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
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
		Debug.Log("Played :" + " " + s.name);
		return s;
	}

    public Sound PlayIt(Sound s)
    {
        if (s == null)
        {
            Debug.LogWarning("Sound not found!");
            return null;
        }
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.loop = s.loop;

        //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        //s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        s.source.Play();
        return s;
    }

    private void Start()
    {
		//audioSource.Play();

		//Play("MainLoop");
    }
}
