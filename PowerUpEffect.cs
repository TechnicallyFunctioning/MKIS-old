using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    private ParticleSystem powerUpParticle;

    private void Start()
    {
        powerUpParticle = transform.Find("PowerUp Particle").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ship1") || other.CompareTag("Ship2"))
        {
            powerUpParticle.Play();
            Destroy(gameObject, .5f);
        }
    }
}
