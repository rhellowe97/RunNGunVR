using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1 )]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    protected bool fullAuto = false;
    public bool FullAuto => fullAuto;

    [SerializeField]
    protected bool hasBurst = false;
    public bool HasBurst => hasBurst;

    [ShowIf( nameof( hasBurst ) )]
    [SerializeField]
    protected int burstAmount = 2;
    public int BurstAmount => burstAmount;

    [ShowIf( nameof( hasBurst ) )]
    [SerializeField]
    protected float burstCooldown = 0.1f;
    public float BurstCooldown => burstCooldown;

    [SerializeField]
    protected bool hasSpread = false;
    public bool HasSpread => hasSpread;

    [ShowIf( nameof( hasSpread ) )]
    [SerializeField]
    protected float spreadAmount = 8f;
    public float SpreadAmount => spreadAmount;

    [ShowIf( nameof( hasSpread ) )]
    [SerializeField]
    protected float spreadAngle = 30f;
    public float SpreadAngle => spreadAngle;

    [SerializeField]
    protected float fireRate = 0.5f;
    public float FireRate => fireRate;

    [SerializeField]
    protected int clipSize = 10;
    public int ClipSize => clipSize;

    [SerializeField]
    protected bool slugBased = false;
    public bool SlugBased => slugBased;

    [SerializeField]
    protected float damage = 10f;
    public float Damage => damage;

    [SerializeField]
    protected float recoilForce = 1f;
    public float RecoilForce => recoilForce;
}
