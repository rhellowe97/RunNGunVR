using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlottableSocketInteractor : XRSocketInteractor
{
    [SerializeField]
    protected List<SlottableType> allowableTypes;

    public override bool CanSelect( IXRSelectInteractable interactable )
    {
        bool canSlot = false;

        SlottableItem slottable = interactable.transform.GetComponent<SlottableItem>();

        canSlot = slottable != null && allowableTypes.Contains( slottable.Type );

        Debug.Log( $"{interactable.transform.gameObject.name}: {canSlot}" );

        return base.CanSelect( interactable ) && canSlot;
    }
}
