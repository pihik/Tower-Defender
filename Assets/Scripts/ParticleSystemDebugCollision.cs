using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDebugCollision : MonoBehaviour
{
    /*
    ParticleSystem arrowPS;
    ParticleSystem.Particle[] particle;
    int numberOfParticles = 0;

    private void Update()
    {
        InitializeIfNeeded();
        numberOfParticles = arrowPS.GetParticles(particle);
    }

    private void OnParticleCollision(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                particle[i].velocity.Set(0, 0, 0);
            }
            
        }
    }
    void InitializeIfNeeded()
    {
        if (arrowPS == null)
        {
            arrowPS = GetComponent<ParticleSystem>();
        }

        if (particle == null || particle.Length < arrowPS.main.maxParticles)
        {
            particle = new ParticleSystem.Particle[arrowPS.main.maxParticles];
        }
    }
    */
}
