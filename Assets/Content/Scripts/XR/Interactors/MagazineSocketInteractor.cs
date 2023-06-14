using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagazineSocketInteractor : SlottableSocketInteractor
{
    public delegate void ReloadDelegate( bool isLoaded );
    public event ReloadDelegate OnReload;

    public Magazine CurrentMag { get; private set; }

    protected override void Start()
    {
        base.Start();

        selectEntered.AddListener( OnLoad );
        selectExited.AddListener( OnUnLoad );

        hoverEntered.AddListener( OnHover );
    }

    private void OnLoad( SelectEnterEventArgs args )
    {
        Magazine newMag = args.interactableObject.transform.GetComponent<Magazine>();

        CurrentMag = newMag;

        newMag.ToggleCollider( false );

        OnReload?.Invoke( true );
    }

    private void OnUnLoad( SelectExitEventArgs args )
    {
        OnReload?.Invoke( false );
    }

    private void OnHover( HoverEnterEventArgs args )
    {
        bool canSlot = false;

        SlottableItem slottable = args.interactableObject.transform.GetComponent<SlottableItem>();

        canSlot = slottable != null && allowableTypes.Contains( slottable.Type );

        if ( canSlot && ( args.interactableObject != null || !( args.interactableObject.interactorsHovering[0] is XRSocketInteractor ) ) )
            EjectMag();
    }

    public bool EjectMag()
    {
        if ( CurrentMag != null )
        {
            interactionManager.CancelInteractableSelection( CurrentMag.Interactable as IXRSelectInteractable );

            CurrentMag.ToggleCollider( false );

            CurrentMag.Interactable.enabled = false;

            CurrentMag.SetMagDespawn();

            CurrentMag = null;

            return true;
        }

        return false;
    }
}
