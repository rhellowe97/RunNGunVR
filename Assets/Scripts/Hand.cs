using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{
    private InputDevice targetDevice;

    [SerializeField]
    private InputDeviceCharacteristics inputCharacteristics;

    [SerializeField]
    private GameObject handPrefab;

    private GameObject spawnedPrefab;

    private Animator handAnimator;

    private bool initalized = false;

    private void Start()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics( inputCharacteristics, devices );

        if ( devices.Count > 0 )
        {
            targetDevice = devices[0];

            spawnedPrefab = Instantiate( handPrefab, transform );

            handAnimator = spawnedPrefab.GetComponent<Animator>();

            HandInteractor handInteractor = GetComponentInParent<HandInteractor>();

            if ( handInteractor )
                handInteractor.AssignHand( transform );

            initalized = true;
        }
    }

    void Update()
    {
        if ( !initalized )
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimations();
        }
    }

    private void UpdateHandAnimations()
    {
        if ( targetDevice.TryGetFeatureValue( CommonUsages.trigger, out float triggerValue ) )
        {
            handAnimator.SetFloat( "Trigger", triggerValue );
        }
        else
        {
            handAnimator.SetFloat( "Trigger", 0 );
        }

        if ( targetDevice.TryGetFeatureValue( CommonUsages.grip, out float gripValue ) )
        {
            handAnimator.SetFloat( "Grip", gripValue );
        }
        else
        {
            handAnimator.SetFloat( "Grip", 0 );
        }
    }
}
