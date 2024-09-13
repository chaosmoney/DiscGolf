using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticleController : MonoBehaviour
{
    public ParticleSystem burstStar;
    public AudioSource sfx;
    public void StartParticle()
    {
        burstStar.Stop();
        burstStar.Play();
    }

    public void StartBGM()
    {
        sfx.Play();
    }
}
