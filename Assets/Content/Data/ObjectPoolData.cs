using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "ObjectPoolData", menuName = "ScriptableObjects/ObjectPoolData", order = 3 )]
public class ObjectPoolData : ScriptableObject
{
    [SerializeField]
    protected List<PooledObjectConfig> pooledObjectConfigs = new List<PooledObjectConfig>();
    public List<PooledObjectConfig> ObjectConfigs => pooledObjectConfigs;
}
