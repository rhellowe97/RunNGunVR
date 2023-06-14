using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField]
    protected Transform head;

    [SerializeField]
    protected Transform leftHand;

    [SerializeField]
    protected Transform rightHand;

    private void MapPosition( Transform target, XRNode node )
    {
        //InputDevice.GetXRDev ( node ).
    }
}
