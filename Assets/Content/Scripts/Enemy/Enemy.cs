using RootMotion.Demos;
using RootMotion.Dynamics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : PooledObject
{
    public delegate void EnemyDeathDelegate( Enemy enemy );
    public event EnemyDeathDelegate OnEnemyDeath;

    [SerializeField]
    protected float health = 100f;

    private float currentHealth;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    protected NavMeshPuppet navPuppet;

    [SerializeField]
    protected PuppetMaster puppetMaster;

    [BoxGroup, ShowIf( nameof( puppetMaster ) )]
    [Tooltip( "The speed of fading out PuppetMaster.pinWeight." )]
    [SerializeField]
    protected float fadeOutPinWeightSpeed = 5f;

    [BoxGroup, ShowIf( nameof( puppetMaster ) )]
    [Tooltip( "The speed of fading out PuppetMaster.muscleWeight." )]
    [SerializeField]
    protected float fadeOutMuscleWeightSpeed = 5f;

    [BoxGroup, ShowIf( nameof( puppetMaster ) )]
    [Tooltip( "The muscle weight to fade out to." )]
    [SerializeField]
    protected float deadMuscleWeight = 0.3f;

    private float startMuscleWeight = 1f;

    private float startPinWeight = 1f;

    [SerializeField]
    protected Renderer deathRenderer;

    [SerializeField]
    protected bool showHealthBar;

    [SerializeField]
    protected Slider healthBar;

    [SerializeField]
    protected float healthBarFadeTime = 4f;

    [SerializeField]
    protected float deathTime = 3f;

    private float healthBarTimer = 0f;

    private Coroutine healthBarCo;

    private bool isDead = false;

    private MaterialPropertyBlock mpb;

    private void Start()
    {
        currentHealth = health;

        if ( puppetMaster != null )
        {
            startPinWeight = puppetMaster.pinWeight;

            startMuscleWeight = puppetMaster.muscleWeight;
        }
    }

    public void TakeDamage( float damage )
    {
        Debug.Log( damage );

        currentHealth = Mathf.Max( currentHealth - damage, 0 );

        if ( healthBar != null )
            healthBar.value = currentHealth / health;

        if ( !isDead && showHealthBar )
        {
            healthBarTimer = 0f;

            if ( healthBarCo == null )
                healthBarCo = StartCoroutine( ShowHealthBar() );
        }

        if ( !isDead && currentHealth <= 0 )
        {
            isDead = true;

            if ( healthBarCo != null )
                StopCoroutine( ShowHealthBar() );

            healthBar.gameObject.SetActive( false );

            StartCoroutine( FadeOutMuscleWeight() );
            StartCoroutine( FadeOutPinWeight() );
            StartCoroutine( Death() );
        }
    }

    public void Spawn( Vector3 pos, Quaternion rot )
    {
        puppetMaster.Teleport( pos, rot, true );

        if ( navPuppet != null )
            navPuppet.SetNavMeshPosition( pos );
    }

    public void Despawn()
    {
        ObjectPool.Instance.ReturnToPool( this );
    }

    private IEnumerator Death()
    {
        float t = 0;

        if ( anim != null )
            anim.enabled = false;

        while ( t < deathTime )
        {
            if ( deathRenderer != null )
            {
                mpb.SetFloat( "_DissolvePercent", Mathf.Lerp( 0, 1, t / deathTime ) );

                deathRenderer.SetPropertyBlock( mpb );
            }

            t += Time.deltaTime;

            yield return null;
        }

        OnEnemyDeath?.Invoke( this );

        ObjectPool.Instance.ReturnToPool( this );
    }

    private IEnumerator ShowHealthBar()
    {
        if ( healthBar == null )
        {
            Debug.LogError( "No Healthbar assigned..." );
            yield break;
        }

        healthBar.gameObject.SetActive( true );

        while ( healthBarTimer < healthBarFadeTime )
        {
            healthBarTimer += Time.deltaTime;

            yield return null;
        }

        healthBar.gameObject.SetActive( false );

        healthBarCo = null;
    }

    private IEnumerator FadeOutPinWeight()
    {
        while ( puppetMaster.pinWeight > 0f )
        {
            puppetMaster.pinWeight = Mathf.MoveTowards( puppetMaster.pinWeight, 0f, Time.deltaTime * fadeOutPinWeightSpeed );
            yield return null;
        }
    }

    // Fading out puppetMaster.muscleWeight to deadMuscleWeight
    private IEnumerator FadeOutMuscleWeight()
    {
        while ( puppetMaster.muscleWeight > 0f )
        {
            puppetMaster.muscleWeight = Mathf.MoveTowards( puppetMaster.muscleWeight, deadMuscleWeight, Time.deltaTime * fadeOutMuscleWeightSpeed );
            yield return null;
        }
    }

    public override void Init()
    {
        currentHealth = health;

        if ( puppetMaster != null )
        {
            startPinWeight = puppetMaster.pinWeight;

            startMuscleWeight = puppetMaster.muscleWeight;
        }

        mpb = new MaterialPropertyBlock();
    }

    public override void ResetObject()
    {
        currentHealth = health;

        healthBar.value = 1;

        healthBar.gameObject.SetActive( false );

        healthBarTimer = 0f;

        isDead = false;

        if ( puppetMaster != null )
        {
            puppetMaster.pinWeight = startPinWeight;

            puppetMaster.muscleWeight = startMuscleWeight;
        }

        if ( anim != null )
            anim.enabled = true;

        if ( deathRenderer != null )
        {
            mpb.SetFloat( "_DissolvePercent", 0 );

            deathRenderer.SetPropertyBlock( mpb );
        }
    }

    public override void Returned()
    {
        StopAllCoroutines();

        base.Returned();
    }
}