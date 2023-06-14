using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualReload : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody barrel;

    [SerializeField]
    protected SlugSocketInteractor barrelReloadInteractor;

    [SerializeField]
    private float lockThreshold = 5f;

    [SerializeField]
    private AudioSource lockedClip;

    private bool isOpen = false;

    private Vector3 defaultPosition;

    private Vector3 defaultRotation;

    private void Awake()
    {
        defaultPosition = barrel.transform.localPosition;
        defaultRotation = barrel.transform.localEulerAngles;
    }

    public void OpenReload()
    {
        barrel.isKinematic = false;
        barrelReloadInteractor.enabled = true;
        isOpen = true;

        if ( lockedClip )
            lockedClip.Play();
    }

    private void FixedUpdate()
    {
        if ( isOpen )
        {
            if ( barrel.velocity.y > lockThreshold )
            {
                barrel.transform.localEulerAngles = Vector3.zero;
                barrel.isKinematic = true;
                barrelReloadInteractor.enabled = false;
                isOpen = false;

                barrel.transform.localPosition = defaultPosition;
                barrel.transform.localEulerAngles = defaultPosition;

                if ( lockedClip )
                    lockedClip.Play();
            }
        }
    }
}
