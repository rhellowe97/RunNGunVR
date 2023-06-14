using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    [SerializeField]
    protected float pulseRate = 1f;

    protected float maxIntensity = 0f;

    protected Light pulseLight;

    private float internalTimer = 0f;

    void Awake()
    {
        pulseLight = GetComponent<Light>();

        if ( !pulseLight )
        {
            enabled = false;

            return;
        }

        maxIntensity = pulseLight.intensity;
    }

    void Update()
    {
        pulseLight.intensity = ( -0.5f * Mathf.Cos( internalTimer * pulseRate * Mathf.PI ) + 0.55f ) * maxIntensity;

        internalTimer += Time.deltaTime;

        if ( internalTimer > 2 )
        {
            internalTimer = 0;
        }
    }
}
