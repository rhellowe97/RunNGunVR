using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowstick : MonoBehaviour
{
    [SerializeField]
    private Renderer glowRend;

    [SerializeField]
    private float glowSpeed = 1f;

    [SerializeField]
    private float glowIntensity = 2f;

    [SerializeField, ColorUsage( true, true )]
    protected Color startColor;


    [SerializeField, ColorUsage( true, true )]
    protected Color glowColor;

    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();

        mpb.SetColor( "_EmissionColor", startColor );

        glowRend.SetPropertyBlock( mpb );
    }

    public void Activate()
    {
        StartCoroutine( Glow() );
    }

    private IEnumerator Glow()
    {
        float t = 0;

        while ( t < glowSpeed )
        {
            mpb.SetColor( "_EmissionColor", Color.Lerp( startColor, glowColor, t / glowSpeed ) );

            glowRend.SetPropertyBlock( mpb );

            t += Time.deltaTime;

            yield return null;
        }
    }
}
