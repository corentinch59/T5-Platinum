using System.Collections;
using System;
using UnityEngine;

public class ParticlePlayManager : MonoBehaviour
{
    public static ParticlePlayManager instance;
    public Pparticle[] particleSystems;

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
    }

    public void PlayAtPosition(string particle)
    {
        Pparticle p = Array.Find(particleSystems, item => item.name == particle);

        if (p == null)
        {
            Debug.LogWarning("Particle: " + particle + " not found!");
            return;
        }

        //p.particleSystem.transform.position = new Vector3(x, y, z);
        p.particleSystem.Play();
    }

    public ParticleSystem MoveParticleInTheWorld(string particle, Vector3 posAdded)
    {
        Pparticle p = Array.Find(particleSystems, item => item.name == particle);

        if (p == null)
        {
            Debug.LogWarning("Particle: " + particle + " not found!");
            return null;
        }

        p.particleSystem.transform.position += posAdded;
        return p.particleSystem;
    }
}
