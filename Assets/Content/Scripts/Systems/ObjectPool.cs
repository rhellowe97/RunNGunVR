using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    protected List<PooledObjectConfig> pooledObjectConfigs = new List<PooledObjectConfig>();

    private Dictionary<PooledType, Stack<PooledObject>> pool = new Dictionary<PooledType, Stack<PooledObject>>();

    private void Awake()
    {
        if ( Instance != null )
        {
            Destroy( gameObject );
            return;
        }

        Instance = this;

        foreach ( PooledObjectConfig objConfig in pooledObjectConfigs )
        {
            for ( int i = 0; i < objConfig.PreInstantiateCount; i++ )
            {
                if ( i >= objConfig.MaxSize )
                    break;

                if ( !pool.ContainsKey( objConfig.PooledPrefab.PooledType ) )
                {
                    pool.Add( objConfig.PooledPrefab.PooledType, new Stack<PooledObject>() );
                }

                PooledObject newPooledObject = Instantiate( objConfig.PooledPrefab );

                newPooledObject.Init();

                newPooledObject.Returned();

                pool[objConfig.PooledPrefab.PooledType].Push( newPooledObject );
            }
        }
    }

    public GameObject GetPooled( PooledType pooledType )
    {
        if ( !pool.ContainsKey( pooledType ) )
        {
            Debug.LogError( "Trying to get non-pooled prefab!" );

            return null;
        }

        if ( pool[pooledType].Count == 0 )
        {
            foreach ( PooledObjectConfig objConfig in pooledObjectConfigs )
            {
                if ( objConfig.PooledPrefab.PooledType == pooledType )
                {
                    PooledObject newPooledObject = Instantiate( objConfig.PooledPrefab.gameObject ).GetComponent<PooledObject>();

                    newPooledObject.Init();

                    return newPooledObject.gameObject;
                }
            }

            Debug.LogError( "Pooled Type not found in original configuration list!" );

            return null;
        }
        else
        {
            PooledObject unusedInstance = pool[pooledType].Pop();

            unusedInstance.gameObject.SetActive( true );

            unusedInstance.ResetObject();

            return unusedInstance.gameObject;
        }
    }

    public void ReturnToPool( PooledObject pooledObject )
    {
        if ( !pool.ContainsKey( pooledObject.PooledType ) )
        {
            Debug.LogError( "Trying to return a non-pooled object!" );

            return;
        }

        pooledObject.Returned();

        pool[pooledObject.PooledType].Push( pooledObject );
    }
}

[System.Serializable]
public class PooledObjectConfig
{
    [SerializeField]
    protected PooledObject pooledPrefab;
    public PooledObject PooledPrefab => pooledPrefab;

    [SerializeField]
    protected int preInstantiateCount = 0;
    public int PreInstantiateCount => preInstantiateCount;

    [SerializeField]
    protected int maxSize = 0;
    public int MaxSize => maxSize;
}

public enum PooledType
{
    Bullet = 0,

    BulletWallImpact = 100,
    BulletFleshImpact,

    Zombie = 200,
    Boomer,
    HeadSpider,

    HandgunMag = 301,
    RifleMag,
    HandcannonMag
}
