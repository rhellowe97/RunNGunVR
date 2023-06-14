using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugWeapon : Weapon
{
    [SerializeField]
    protected SlugSocketInteractor slugSlot;

    private int ammoCount = 0;

    protected override void Awake()
    {
        base.Awake();

        slugSlot.OnReload += AmmoLoaded;
    }

    protected override bool AmmoCheck()
    {
        if ( ammoCount == 0 )
        {
            if ( emptyAudio != null )
                emptyAudio.Play();

            return false;
        }

        return true;
    }

    protected override void AmmoConsumed()
    {
        ammoCount--;
    }

    public override void AmmoLoaded()
    {
        if ( ammoCount < WeaponData.ClipSize )
        {
            ammoCount++;

            if ( reloadAudio )
            {
                reloadAudio.Play();
            }
        }
    }
}
