using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    } 

    public void ActiveParticle(Color color)
    {
        _particleSystem.startColor = color;
        _particleSystem.Play();
    }
}
