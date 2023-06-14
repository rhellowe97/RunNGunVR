using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    protected float explosionTimer = 4f;

    [SerializeField]
    protected Rigidbody pin;

    [SerializeField]
    protected PooledType explosionEffect;

    private bool activated = false;

    public void Activate()
    {
        if ( !activated )
        {
            activated = true;

            pin.isKinematic = false;

            pin.AddForce( 3f * pin.transform.up, ForceMode.Impulse );

            pin.AddTorque( 3f * new Vector3( Random.Range( -1, 1 ), Random.Range( -1, 1 ), Random.Range( -1, 1 ) ), ForceMode.Impulse );

            StartCoroutine( Countdown() );
        }
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds( explosionTimer );

        PooledEffect explosion = ObjectPool.Instance.GetPooled( explosionEffect ).GetComponent<PooledEffect>();

        if ( explosion )
        {
            explosion.transform.position = transform.position;

            explosion.Play();
        }

        gameObject.SetActive( false ); //Return to pool?
    }
}
