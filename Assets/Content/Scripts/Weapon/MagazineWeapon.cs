using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineWeapon : Weapon
{
    [SerializeField]
    protected MagazineSocketInteractor magSlot;

    protected override void Awake()
    {
        base.Awake();

        magSlot.OnReload += MagLoaded;
    }

    protected override bool AmmoCheck()
    {
        if ( magSlot.CurrentMag == null || magSlot.CurrentMag.Capacity <= 0 )
        {
            if ( magSlot.EjectMag() )
            {
                if ( ejectAudio != null )
                    ejectAudio.Play();
            }
            else
            {
                if ( emptyAudio != null )
                    emptyAudio.Play();
            }

            return false;
        }

        return true;
    }

    protected override void AmmoConsumed()
    {
        if ( magSlot.CurrentMag )
            magSlot.CurrentMag.Capacity--;
    }

    public override void AmmoLoaded()
    {
        if ( !magSlot.CurrentMag.Used )
        {
            magSlot.CurrentMag.Capacity = WeaponData.ClipSize;

            magSlot.CurrentMag.Used = true;
        }

        if ( reloadAudio )
        {
            reloadAudio.Play();
        }
    }

    private void MagLoaded( bool isLoaded )
    {
        if ( isLoaded )
        {
            AmmoLoaded();
        }
        else
        {
            //Manual Unload
        }
    }
}
