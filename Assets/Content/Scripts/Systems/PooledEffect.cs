using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledEffect : PooledObject
{
    [SerializeField]
    private ParticleSystem effect;

    public override void Init()
    {

    }

    public override void ResetObject()
    {

    }

    public void Play()
    {
        effect.Play();
    }

    public void OnParticleSystemStopped()
    {
        ObjectPool.Instance.ReturnToPool( this );
    }
}
