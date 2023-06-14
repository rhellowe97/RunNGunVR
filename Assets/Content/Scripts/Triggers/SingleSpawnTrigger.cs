using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpawnTrigger : ShotTrigger
{
    [SerializeField]
    protected PooledType enemyType;

    [SerializeField]
    protected Transform spawnZone;

    [Button]
    public override void TriggerMechanic()
    {
        Enemy enemy = ObjectPool.Instance.GetPooled( enemyType ).GetComponent<Enemy>();

        enemy.Spawn( spawnZone.position, spawnZone.rotation );
    }
}
