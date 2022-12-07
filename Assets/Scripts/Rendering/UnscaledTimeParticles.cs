using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to have a particule system to works even when timescale in game is set to 0 (game pause)
/// </summary>
public class UnscaledTimeParticles : MonoBehaviour
{
    //Particule system to be not affected
    private ParticleSystem particleSystem;

    /// <summary>
    /// Populate variable with attache particle system
    /// </summary>
    private void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// If time scale is lower than 0.01 (game pause) have the particule system function anyway 
    /// </summary>
    void Update()
    {
        if (Time.timeScale < 0.05f)
        {
            particleSystem.Simulate(Time.unscaledDeltaTime, true, false);
        }
        else
        {
            particleSystem.Simulate(Time.deltaTime, true, false);
        }
    }
}

