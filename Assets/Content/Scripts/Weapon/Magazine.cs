using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : PooledObject
{
    public CustomGrabInteractable Interactable { get; private set; }

    public int Capacity = 0;

    public bool Used = false;

    private WaitForSeconds despawnDelay;

    private Coroutine despawnCo;

    private Collider col;

    public override void Init()
    {
        Interactable = GetComponent<CustomGrabInteractable>();

        despawnDelay = new WaitForSeconds( 3 );
        col = GetComponent<Collider>();
    }

    public void ToggleCollider( bool toggle )
    {
        col.enabled = toggle;
    }

    public override void ResetObject()
    {
        Interactable.enabled = true;

        Used = false;

        ToggleCollider( true );
    }

    public override void Returned()
    {
        if ( despawnCo != null )
        {
            StopCoroutine( despawnCo );
        }

        base.Returned();
    }

    public void SetMagDespawn( bool instant = false )
    {
        despawnCo = StartCoroutine( MagDespawn( instant ) );
    }

    private IEnumerator MagDespawn( bool instant = false )
    {
        if ( instant )
            yield return null;
        else
            yield return despawnDelay;

        ObjectPool.Instance.ReturnToPool( this );
    }
}

