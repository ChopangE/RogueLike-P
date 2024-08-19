using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = this.gameObject.GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particle.Play();
    }

    private void Update()
    {
        if (particle.isStopped)
            EndParticle(); 
    }


    public void EndParticle()
    {
        Manager.Pool.Push(this.gameObject);
    }
}
