using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandInteractor : XRDirectInteractor
{
    protected Transform handModel;

    protected Vector3 initialScale;

    protected CustomGrabInteractable currentItem;

    private XRController controller;

    [BoxGroup( "Item Use" )]
    [SerializeField]
    protected InputActionReference primaryAction;

    [BoxGroup( "Item Use" )]
    [SerializeField]
    protected InputActionReference secondaryAction;

    [BoxGroup( "Item Use" )]
    [SerializeField]
    protected InputActionReference tertiaryAction;

    protected override void Awake()
    {
        base.Awake();

        selectEntered.AddListener( ItemPickup );
        selectExited.AddListener( ItemDropped );

        controller = GetComponent<XRController>();
    }

    protected override void OnEnable()
    {
        if ( primaryAction )
        {
            primaryAction.action.performed += ItemPrimaryDown;
            primaryAction.action.canceled += ItemPrimaryUp;
        }

        if ( secondaryAction )
        {
            secondaryAction.action.performed += ItemSecondaryDown;
            secondaryAction.action.canceled += ItemSecondaryUp;
        }

        if ( tertiaryAction )
        {
            tertiaryAction.action.performed += ItemTertiaryDown;
            tertiaryAction.action.canceled += ItemTertiaryUp;
        }

        base.OnEnable();
    }

    private void ItemPickup( SelectEnterEventArgs args )
    {
        if ( args.interactableObject is CustomGrabInteractable )
        {
            currentItem = (CustomGrabInteractable)args.interactableObject;
            SetHandParent( currentItem.attachTransform, attachTransform );
        }

    }

    private void ItemDropped( SelectExitEventArgs args )
    {
        currentItem = null;

        ReturnHand();
    }

    public void AssignHand( Transform hand )
    {
        handModel = hand;

        initialScale = handModel.localScale;
    }

    public void SetHandParent( Transform newParent, Transform localOffset = null )
    {
        if ( handModel )
        {
            handModel.SetParent( newParent );
            if ( localOffset )
            {
                handModel.localPosition = -localOffset.localPosition;
                handModel.localEulerAngles = -localOffset.localEulerAngles;
            }
            else
            {
                handModel.localPosition = Vector3.zero;
                handModel.localRotation = Quaternion.identity;
            }
        }

    }

    public void ReturnHand()
    {
        SetHandParent( transform );

        handModel.localScale = initialScale;
    }

    private void ItemPrimaryDown( InputAction.CallbackContext context )
    {
        if ( currentItem )
        {
            currentItem.UsePrimary();

        }
    }

    private void ItemPrimaryUp( InputAction.CallbackContext context )
    {
        if ( currentItem )
            currentItem.EndPrimary();
    }

    private void ItemSecondaryDown( InputAction.CallbackContext context )
    {
        if ( currentItem )
            currentItem.UseSecondary();
    }

    private void ItemSecondaryUp( InputAction.CallbackContext context )
    {
        if ( currentItem )
            currentItem.EndSecondary();
    }

    private void ItemTertiaryDown( InputAction.CallbackContext context )
    {
        if ( currentItem )
            currentItem.UseTertiary();
    }

    private void ItemTertiaryUp( InputAction.CallbackContext context )
    {
        if ( currentItem )
            currentItem.EndTertiary();
    }

    protected override void OnDisable()
    {
        if ( primaryAction )
        {
            primaryAction.action.performed -= ItemPrimaryDown;
            primaryAction.action.canceled -= ItemPrimaryUp;
        }

        if ( secondaryAction )
        {
            secondaryAction.action.performed -= ItemSecondaryDown;
            secondaryAction.action.canceled -= ItemSecondaryUp;
        }

        if ( tertiaryAction )
        {
            tertiaryAction.action.performed -= ItemTertiaryDown;
            tertiaryAction.action.canceled -= ItemTertiaryUp;
        }

        base.OnDisable();
    }
}
