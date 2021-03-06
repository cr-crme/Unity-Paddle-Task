using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// contains necessary effect information and setup. can have a shader and a set of particle effects
/// </summary>
public class VideoEffect : MonoBehaviour
{
    public float effectTime = 4;
    public AnimationCurve fadeIn;
    public Material material;
    public List<EffectParticle> effectParticles = new List<EffectParticle>();
    [NonSerialized]
    public ParticleSystem ps;
    new public Renderer renderer;
    [NonSerialized]
    public int shaderProperty;
    public Vector3 localOffset = Vector3.zero;

    float timer = 0;

    bool playing;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        shaderProperty = Shader.PropertyToID("_cutoff");
        ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.duration = effectTime;
        }
    }

    public void SetEffectProperties(
        float effectTimeVar, 
        AnimationCurve fadeInVar,
        Material materialVar, 
        ParticleSystem psVar, 
        int shaderPropertyVar
    )
    {
        effectTime = effectTimeVar;
        fadeIn = fadeInVar;
        material = materialVar;
        ps = psVar;
        shaderProperty = shaderPropertyVar;
    }

    void Update()
    {
        if (playing)
        {
            if (timer < effectTime)
            {
                timer += Time.deltaTime;
                renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, effectTime, timer)));
            }
            else
            {
                StopEffect();
            }
        }
    }

    public void StopEffect()
    {
        playing = false;
        timer = -1;
    }

    public void StartEffect()
    {
        if (ps == null)
            return;

        playing = true;
        ps.Play();
        timer = 0;
    }

    public EffectParticle GetEffectParticle(VideoEffect effect)
    {
        foreach (var particle in effectParticles)
        {
            if (particle.effect == effect)
            {
                return particle;
            }
        }
        return null;
    }

}
