using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "WaveData", menuName = "ScriptableObjects/WaveData", order = 2 )]
public class WaveData : ScriptableObject
{
    [SerializeField]
    protected List<Wave> waves = new List<Wave>();
    public List<Wave> Waves => waves;
}

[System.Serializable]
public class WaveEnemyInstance
{
    [SerializeField]
    protected PooledType enemyType;
    public PooledType EnemyType => enemyType;

    [SerializeField]
    protected int enemyCount = 0;
    public int EnemyCount => enemyCount;
}

[System.Serializable]
public class Wave
{
    [SerializeField]
    protected float spawnRate = 3;
    public float SpawnRate => spawnRate;

    [SerializeField]
    protected List<WaveEnemyInstance> enemyList = new List<WaveEnemyInstance>();
    public List<WaveEnemyInstance> EnemyList => enemyList;
}
