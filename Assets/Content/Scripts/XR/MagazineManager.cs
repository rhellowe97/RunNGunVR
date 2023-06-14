using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MagazineManager : MonoBehaviour
{
    private SlottableSocketInteractor magSocket;

    private PooledType currentMagType = PooledType.HandgunMag;

    private CustomGrabInteractable currentMag;

    private void Awake()
    {
        magSocket = GetComponent<SlottableSocketInteractor>();

        Weapon.OnWeaponSelected += ( PooledType newMagType ) =>
        {
            bool sameMag = ( currentMagType == newMagType );

            currentMagType = newMagType;

            if ( !sameMag )
                RemoveCurrentStoredMag();

            RespawnCurrentMag();
        };
    }

    private void Start()
    {
        if ( currentMag != null )
            RespawnCurrentMag();
    }

    public void RemoveCurrentStoredMag()
    {
        if ( currentMag != null )
        {
            Magazine disposedMag = currentMag.GetComponent<Magazine>();

            currentMag.interactionManager.CancelInteractableSelection( currentMag as IXRSelectInteractable );

            disposedMag.Interactable.enabled = false;

            disposedMag.SetMagDespawn();
        }
    }

    public void RespawnCurrentMag()
    {
        currentMag = ObjectPool.Instance.GetPooled( currentMagType ).GetComponent<CustomGrabInteractable>();

        currentMag.transform.position = magSocket.transform.position;

        currentMag.interactionManager.SelectEnter( magSocket, currentMag as IXRSelectInteractable );
    }
}
