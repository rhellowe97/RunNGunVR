using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using Unity.XR.CoreUtils;

public class LocomotionController : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider body;

    [SerializeField]
    private float additionalCollisionHeight = 0.2f;

    [SerializeField]
    private XRController teleportRay;

    [SerializeField]
    private InputHelpers.Button teleportButton;

    [SerializeField]
    private float activationThreshold = 0.1f;

    protected Volume volume;

    [SerializeField]
    protected float vignetteIntensity = 0.45f;

    [SerializeField]
    protected float vignetteDuration = 0.5f;

    [SerializeField]
    protected float vignetteThreshold = 0.5f;

    private bool isMoving = true;

    private Tween vignetteTween = null;

    private Vignette vignette;

    private float intensity = 1;

    private Vector3 lastPos;

    private XROrigin rig;

    private ContinuousMoveProviderBase moveProvider;

    private void Awake()
    {
        rig = GetComponent<XROrigin>();

        volume = FindObjectOfType<Volume>();

        moveProvider = GetComponent<ContinuousMoveProviderBase>();

        if ( volume.profile.TryGet( out Vignette profVignette ) )
        {
            vignette = profVignette;
        }

        vignette.intensity.Override( 0 );

        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( teleportRay )
        {
            InputHelpers.IsPressed( teleportRay.inputDevice, teleportButton, out bool active, activationThreshold );

            teleportRay.gameObject.SetActive( active );
        }

        CheckForMovement();
    }

    private void CheckForMovement()
    {
        float sqrSpeed = ( ( lastPos - transform.position ).sqrMagnitude / Time.deltaTime ) / Time.deltaTime;

        if ( sqrSpeed > vignetteThreshold * vignetteThreshold && !isMoving )
        {
            BeginLocomotion();
            isMoving = true;
        }
        else if ( sqrSpeed < vignetteThreshold * vignetteThreshold && isMoving )
        {
            EndLocomotion();
            isMoving = false;
        }

        vignette.intensity.Override( intensity );

        lastPos = transform.position;
    }

    private void BeginLocomotion()
    {
        if ( vignetteTween != null )
        {
            vignetteTween.Kill();
        }

        vignetteTween = DOTween.To( () => intensity, x => intensity = x, vignetteIntensity, vignetteDuration );
    }

    private void EndLocomotion()
    {
        if ( vignetteTween != null )
        {
            vignetteTween.Kill();
        }

        vignetteTween = DOTween.To( () => intensity, x => intensity = x, 0f, vignetteDuration );
    }

    private void FixedUpdate()
    {
        body.height = rig.CameraInOriginSpaceHeight + additionalCollisionHeight;

        body.transform.position = new Vector3( rig.Camera.gameObject.transform.position.x, transform.position.y + body.height / 2 + 0.08f, rig.Camera.gameObject.transform.position.z );
    }
}
