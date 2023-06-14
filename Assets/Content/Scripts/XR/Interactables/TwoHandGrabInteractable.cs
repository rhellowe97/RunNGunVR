using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : CustomGrabInteractable
{
    [SerializeField]
    private List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;

    private Quaternion initialLocalRotation;

    protected enum TwoHandType { None, First, Second }

    [SerializeField]
    protected TwoHandType twoHandType = TwoHandType.None;

    // Start is called before the first frame update
    void Start()
    {
        foreach ( XRSimpleInteractable item in secondHandGrabPoints )
        {
            item.selectEntered.AddListener( ( SelectEnterEventArgs args ) => OnSecondHandGrab( args.interactor, args.interactable.transform ) );
            item.selectExited.AddListener( ( SelectExitEventArgs args ) => OnSecondHandRelease( args.interactor ) );
        }
    }

    public override void ProcessInteractable( XRInteractionUpdateOrder.UpdatePhase updatePhase )
    {
        if ( secondInteractor && selectingInteractor )
        {
            selectingInteractor.attachTransform.rotation = GetTwoHandedRotation();
        }

        base.ProcessInteractable( updatePhase );
    }

    private Quaternion GetTwoHandedRotation()
    {
        Quaternion handRot = Quaternion.identity;

        switch ( twoHandType )
        {
            case TwoHandType.None:
                handRot = Quaternion.LookRotation( secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position );
                break;
            case TwoHandType.First:
                handRot = Quaternion.LookRotation( secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up );
                break;
            case TwoHandType.Second:
                handRot = Quaternion.LookRotation( secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up );
                break;
        }

        return handRot;
    }

    public void OnSecondHandGrab( XRBaseInteractor interactor, Transform attachPoint )
    {
        secondInteractor = interactor;

        if ( secondInteractor is HandInteractor )
        {
            ( secondInteractor as HandInteractor ).SetHandParent( attachPoint );
        }

    }

    public void OnSecondHandRelease( XRBaseInteractor interactor )
    {
        if ( selectingInteractor )
            selectingInteractor.attachTransform.localRotation = initialLocalRotation;

        if ( secondInteractor is HandInteractor )
            ( secondInteractor as HandInteractor ).ReturnHand();

        secondInteractor = null;
    }

    protected override void OnPickup( SelectEnterEventArgs args )
    {
        initialLocalRotation = args.interactor.attachTransform.localRotation;

        base.OnPickup( args );
    }

    protected override void OnDrop( SelectExitEventArgs args )
    {
        args.interactor.attachTransform.localRotation = initialLocalRotation;

        if ( secondInteractor )
            OnSecondHandRelease( secondInteractor );

        base.OnDrop( args );
    }
}
