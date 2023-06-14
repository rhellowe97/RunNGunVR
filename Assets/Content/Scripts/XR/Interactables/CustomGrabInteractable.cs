using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrabInteractable : XRGrabInteractable, IXRSelectInteractable
{
    [BoxGroup( "Item Haptics" )]
    [SerializeField]
    protected bool usePrimaryHaptics = false;
    public bool UsePrimaryHaptics => usePrimaryHaptics;

    [BoxGroup( "Item Haptics" )]
    [SerializeField, Range( 0f, 1f )]
    protected float primaryHapticIntensity = 0f;
    public float PrimaryHapticIntensity => primaryHapticIntensity;

    [BoxGroup( "Item Haptics" )]
    [SerializeField]
    protected float primaryHapticDuration = 0f;
    public float PrimaryHapticDuration => primaryHapticDuration;

    [BoxGroup( "Item Haptics" )]
    [SerializeField]
    protected bool useSecondaryHaptics = false;
    public bool UseSecondaryHaptics => useSecondaryHaptics;

    [BoxGroup( "Item Haptics" )]
    [SerializeField, Range( 0f, 1f )]
    protected float secondaryHapticIntensity = 0f;
    public float SecondaryHapticIntensity => secondaryHapticIntensity;

    [BoxGroup( "Item Haptics" )]
    [SerializeField]
    protected float secondaryHapticDuration = 0f;
    public float SecondaryHapticDuration => secondaryHapticDuration;

    [BoxGroup( "Item Haptics" )]
    [SerializeField]
    protected bool useTertiaryHaptics = false;
    public bool UseTertiaryHaptics => useTertiaryHaptics;

    [BoxGroup( "Item Haptics" )]
    [SerializeField, Range( 0f, 1f )]
    protected float tertiaryHapticIntensity = 0f;
    public float TertiaryHapticIntensity => tertiaryHapticIntensity;

    [BoxGroup( "Item Haptics" )]
    [SerializeField]
    protected float tertiaryHapticDuration = 0f;
    public float TertiaryHapticDuration => tertiaryHapticDuration;


    [BoxGroup( "Item Events" )]
    [SerializeField]
    protected UnityEvent PrimaryUsageDown, PrimaryUsageUp, SecondaryUsageDown, SecondaryUsageUp, TertiaryUsageDown, TertiaryUsageUp;

    [SerializeField]
    protected XRSocketInteractor holster;

    protected bool holstered = false;

    protected override void Awake()
    {
        base.Awake();

        selectEntered.AddListener( OnPickup );
        selectExited.AddListener( OnDrop );

        if ( holster != null )
        {
            transform.position = holster.transform.position;

            holstered = true;
        }
    }

    protected virtual void OnPickup( SelectEnterEventArgs args )
    {
        holstered = false;
    }

    protected virtual void OnDrop( SelectExitEventArgs args )
    {
        if ( holster != null )
        {
            transform.position = holster.transform.position;

            holstered = true;
        }
    }

    public override bool IsSelectableBy( IXRSelectInteractor interactor )
    {
        bool isAlreadyGrabbed = firstInteractorSelecting != null && !( firstInteractorSelecting is XRSocketInteractor ) && !interactor.Equals( firstInteractorSelecting );

        return base.IsSelectableBy( interactor as IXRSelectInteractor ) && !isAlreadyGrabbed;
    }

    public void UsePrimary()
    {
        PrimaryUsageDown?.Invoke();
    }

    public void EndPrimary()
    {
        PrimaryUsageUp?.Invoke();
    }

    public void UseSecondary()
    {
        SecondaryUsageDown?.Invoke();
    }

    public void EndSecondary()
    {
        SecondaryUsageUp?.Invoke();
    }

    public void UseTertiary()
    {
        TertiaryUsageDown?.Invoke();
    }

    public void EndTertiary()
    {
        TertiaryUsageUp?.Invoke();
    }
}
