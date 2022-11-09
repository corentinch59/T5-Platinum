using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ActivateCollisonParticle : MonoBehaviour
{
    [Header("Particules")]
    [SerializeField] private ParticleSystem particleEmitter;

    //[Header("Goes to")]
    ////[SerializeField] private Vector3 posTarget;
    //[SerializeField] private Transform target;
    //[SerializeField] private float speed;

    [Tooltip("The time before the particles start moving in second")]
    [SerializeField] private float TimeBeforeActivation = 1f;


    private Particle[] particles;

    private float elapsedTime;

    private CollisionModule collision;

    private void Start()
    {
        InitializeIfNeeded();
        collision = particleEmitter.collision;
        collision.enabled = false;
    }

    private void LateUpdate()
    {
        int numParticlesAlive = particleEmitter.GetParticles(particles);
        elapsedTime += Time.deltaTime;

        if (elapsedTime > TimeBeforeActivation)
        {
            collision.enabled = true;
            //for (int i = 0; i < numParticlesAlive; i++)
            //{
            //    int ratio = i / numParticlesAlive;
            //    particles[i].position = Vector3.MoveTowards(particles[i].position, target.position, Time.deltaTime * speed);

            //}

            // Apply the particle changes to the Particle System
            particleEmitter.SetParticles(particles, numParticlesAlive);
        }

        if (numParticlesAlive == 0)
        {
            elapsedTime = 0f;
        }

        // Change only the particles that are alive
    }

    private void InitializeIfNeeded()
    {
        if (particleEmitter == null)
            particleEmitter = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < particleEmitter.main.maxParticles)
            particles = new ParticleSystem.Particle[particleEmitter.main.maxParticles];
    }

}
