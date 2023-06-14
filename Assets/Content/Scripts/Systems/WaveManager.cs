using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int aliveCount = 0;

    [SerializeField]
    protected WaveData waveData;

    [SerializeField]
    protected List<Transform> spawns = new List<Transform>();

    [SerializeField]
    protected AudioSource waveStartCue;

    [SerializeField]
    protected GameObject nextWavePrompt;

    private Coroutine roundCo;

    private List<Enemy> activeEnemies = new List<Enemy>();

    private bool nextWaveActive = true;

    public void StartRound()
    {
        if ( waveData == null )
        {
            Debug.LogError( "Missing WaveData..." );
            return;
        }
        else if ( spawns.Count == 0 )
        {
            Debug.LogError( "No Spawns..." );
            return;
        }
        else if ( ObjectPool.Instance == null )
        {
            Debug.LogError( "No Object Pool..." );
            return;
        }

        if ( waveStartCue )
            waveStartCue.Play();

        StopRound();

        roundCo = StartCoroutine( RoundLoop() );
    }

    public void StartNextWave()
    {
        nextWaveActive = true;

        if ( waveStartCue )
            waveStartCue.Play();
    }

    private void SpawnEnemy( PooledType type )
    {
        aliveCount++;

        Enemy enemy = ObjectPool.Instance.GetPooled( type ).GetComponent<Enemy>();

        int spawnIndex = Random.Range( 0, spawns.Count );

        enemy.Spawn( spawns[spawnIndex].position, spawns[spawnIndex].rotation );

        enemy.OnEnemyDeath += EnemyDead;

        activeEnemies.Add( enemy );
    }

    private void EnemyDead( Enemy enemy )
    {
        aliveCount--;

        enemy.OnEnemyDeath -= EnemyDead;

        activeEnemies.Remove( enemy );
    }

    public void StopRound()
    {
        if ( roundCo != null )
        {
            foreach ( Enemy enemy in activeEnemies )
            {
                enemy.OnEnemyDeath -= EnemyDead;

                enemy.Despawn();
            }

            activeEnemies.Clear();

            StopCoroutine( roundCo );
        }
    }

    private IEnumerator RoundLoop()
    {
        float spawnTimer = 0f;

        foreach ( Wave wave in waveData.Waves )
        {
            yield return new WaitUntil( () => nextWaveActive );

            nextWavePrompt.SetActive( false );

            nextWaveActive = false;

            int waveTotal = 0;

            int totalSpawned = 0;

            for ( int i = 0; i < wave.EnemyList.Count; i++ )
            {
                waveTotal += wave.EnemyList[i].EnemyCount;
            }

            yield return new WaitForSeconds( 5 );

            while ( totalSpawned < waveTotal )
            {
                spawnTimer = 0f;

                SpawnEnemy( wave.EnemyList[Random.Range( 0, wave.EnemyList.Count )].EnemyType );

                totalSpawned++;

                while ( spawnTimer <= wave.SpawnRate )
                {
                    spawnTimer += Time.deltaTime;

                    yield return null;
                }
            }

            while ( aliveCount > 0 )
            {
                yield return null;
            }

            //Show Next Wave UI?
            nextWavePrompt.SetActive( true );

        }

        yield return null;
    }
}
