using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledObject
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float unpin = 10f;

    [SerializeField]
    private float impactForce = 10f;

    [SerializeField]
    private float lifeDuration = 5f;

    [SerializeField]
    private LayerMask environmentLayerMask;

    [SerializeField]
    private LayerMask enemyLayerMask;

    private float distanceChunk = 0;

    private float travelledDistance = 0;

    private RaycastHit collisionRayHit;

    private float lifeTimer = 0f;

    public float Damage { get; private set; }

    private TrailRenderer projTrail;

    public void PrepareProjectile( float damage )
    {
        Damage = damage;
    }

    public override void Init()
    {
        distanceChunk = speed * Time.fixedDeltaTime;

        projTrail = GetComponent<TrailRenderer>();
    }

    public override void ResetObject()
    {
        travelledDistance = 0f;

        lifeTimer = 0f;
    }

    public override void Returned()
    {
        if ( projTrail != null )
        {
            projTrail.Clear();
        }

        base.Returned();
    }

    private void FixedUpdate()
    {
        Vector3 rayStartPosition = transform.position - ( transform.forward * Mathf.Min( travelledDistance, distanceChunk ) );

        if ( Physics.Raycast( rayStartPosition, transform.forward, out collisionRayHit, 2 * distanceChunk, environmentLayerMask | enemyLayerMask ) )
        {
            if ( collisionRayHit.collider.attachedRigidbody != null )
            {
                MuscleCollisionBroadcaster broadcaster = collisionRayHit.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();

                if ( broadcaster != null )
                {
                    broadcaster.Hit( unpin, transform.forward * impactForce, collisionRayHit.point );

                    Enemy enemy = broadcaster.GetComponentInParent<Enemy>();

                    if ( enemy != null )
                    {
                        enemy.TakeDamage( Damage );
                    }
                }
            }
            else
            {
                Enemy enemy = collisionRayHit.collider.GetComponentInParent<Enemy>();

                if ( enemy != null )
                {
                    enemy.TakeDamage( Damage );
                }
                else
                {
                    ShotTrigger trigger = collisionRayHit.collider.GetComponent<ShotTrigger>();

                    if ( trigger != null )
                    {
                        trigger.TriggerMechanic();
                    }
                }


            }


            PooledEffect impactEffect;

            if ( enemyLayerMask == ( enemyLayerMask | 1 << collisionRayHit.collider.gameObject.layer ) )
            {
                impactEffect = ObjectPool.Instance.GetPooled( PooledType.BulletWallImpact ).GetComponent<PooledEffect>();
            }
            else
            {
                impactEffect = ObjectPool.Instance.GetPooled( PooledType.BulletWallImpact ).GetComponent<PooledEffect>();
            }

            impactEffect.transform.position = collisionRayHit.point;

            impactEffect.transform.rotation = Quaternion.LookRotation( collisionRayHit.normal, Vector3.up );

            impactEffect.Play();

            ObjectPool.Instance.ReturnToPool( this );
        }
        else
        {
            transform.position += transform.forward * distanceChunk;

            if ( travelledDistance < distanceChunk )
                travelledDistance += distanceChunk;
        }

        lifeTimer += Time.fixedDeltaTime;

        if ( lifeTimer >= lifeDuration )
            ObjectPool.Instance.ReturnToPool( this );

    }
}
