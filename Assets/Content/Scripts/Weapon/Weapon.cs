using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class Weapon : MonoBehaviour
{
    public delegate void WeaponSelected( PooledType magType );
    public static event WeaponSelected OnWeaponSelected;

    [SerializeField]
    protected Animator weaponAnim;

    [SerializeField]
    protected WeaponData WeaponData;

    [SerializeField]
    protected ParticleSystem fireSystem;

    [SerializeField]
    protected PooledType bulletType;

    [SerializeField]
    protected PooledType ammoType;

    [SerializeField]
    protected AudioSource fireAudio;

    [SerializeField]
    protected AudioSource ejectAudio;

    [SerializeField]
    protected AudioSource emptyAudio;

    [SerializeField]
    protected AudioSource reloadAudio;

    protected Rigidbody rb;

    protected float fireTimer = 0f;

    protected bool isFiring = false;

    protected XRBaseInteractable interactable;

    protected float trueFireCooldown = 0f;

    protected Coroutine burstCo;

    protected WaitForSeconds burstDelay;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();

        interactable = GetComponent<XRBaseInteractable>();

        trueFireCooldown = WeaponData.FireRate + ( WeaponData.HasBurst ? WeaponData.BurstAmount * WeaponData.BurstCooldown : 0 );

        burstDelay = new WaitForSeconds( WeaponData.BurstCooldown );
    }

    void Update()
    {
        if ( isFiring )
        {
            if ( fireTimer >= trueFireCooldown )
            {
                Fire();

                fireTimer = 0f;
            }
        }

        if ( WeaponData && fireTimer <= trueFireCooldown )
            fireTimer += Time.deltaTime;
    }

    public void OnTryFire()
    {
        Debug.Log( "Fire" );

        if ( WeaponData == null )
        {
            Debug.LogError( "Weapon is missing Data!" );

            return;
        }

        if ( fireTimer >= trueFireCooldown )
        {
            Fire();

            fireTimer = 0f;
        }

        isFiring = WeaponData.FullAuto;
    }

    public void OnFireEnd()
    {
        isFiring = false;
    }

    protected void Fire()
    {
        if ( !AmmoCheck() )
            return;

        if ( weaponAnim )
            weaponAnim.SetTrigger( "Fire" );

        if ( WeaponData.HasBurst )
        {
            burstCo = StartCoroutine( BurstFireCo() );
        }
        else
        {
            if ( WeaponData.HasSpread )
                FireBulletSpread();
            else
                FireSingleBullet();
        }

        //magSlot.CurrentMag.Capacity--;
    }

    private void FireSingleBullet()
    {
        Projectile bullet = ObjectPool.Instance.GetPooled( bulletType ).GetComponent<Projectile>();

        bullet.transform.position = fireSystem.transform.position;

        bullet.transform.rotation = fireSystem.transform.rotation;

        bullet.PrepareProjectile( WeaponData.Damage );

        OnFired();
    }

    private void FireBulletSpread()
    {
        float degChunk = 360 / WeaponData.SpreadAmount;

        for ( int i = 0; i < WeaponData.SpreadAmount; i++ )
        {
            Vector2 coord = Vector2.zero;

            coord.x = Mathf.Cos( i * degChunk );

            coord.y = Mathf.Sin( i * degChunk );

            coord *= Random.Range( 0.2f, 0.8f );

            Projectile bullet = ObjectPool.Instance.GetPooled( bulletType ).GetComponent<Projectile>();

            bullet.transform.position = fireSystem.transform.position;

            bullet.transform.rotation = fireSystem.transform.rotation;

            bullet.transform.Rotate( coord * WeaponData.SpreadAngle, Space.Self );

            bullet.PrepareProjectile( WeaponData.Damage );
        }

        OnFired();
    }

    private void OnFired()
    {
        fireSystem.Play();

        rb.AddForceAtPosition( transform.up, fireSystem.transform.position, ForceMode.Impulse );

        fireAudio.Play();

        AmmoConsumed();
    }

    private IEnumerator BurstFireCo()
    {
        for ( int i = 0; i < WeaponData.BurstAmount; i++ )
        {
            if ( WeaponData.HasSpread )
                FireBulletSpread();
            else
                FireSingleBullet();

            yield return burstDelay;
        }
    }

    protected abstract bool AmmoCheck();

    protected abstract void AmmoConsumed();

    public abstract void AmmoLoaded();

    //public void MagLoaded( bool isLoaded )
    //{
    //    if ( isLoaded )
    //    {
    //        if ( !magSlot.CurrentMag.Used )
    //        {
    //            magSlot.CurrentMag.Capacity = WeaponData.ClipSize;

    //            magSlot.CurrentMag.Used = true;
    //        }

    //        if ( reloadAudio )
    //        {
    //            reloadAudio.Play();
    //        }
    //    }
    //    else
    //    {
    //        //Manual Unload
    //    }
    //}

    public void WeaponPickedUp()
    {
        if ( interactable.firstInteractorSelecting != null && !( interactable.firstInteractorSelecting is XRSocketInteractor ) )
        {
            OnWeaponSelected?.Invoke( ammoType );

            Debug.Log( "PICKED UP" );
        }
    }

    public void WeaponDropped()
    {
        if ( burstCo != null )
            StopCoroutine( burstCo );
    }
}
