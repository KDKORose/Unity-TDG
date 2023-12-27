using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public List<GameObject> enemyPrefabs;

    private GameObject[] towers;
    public float timeBetweenWaves = 10f;
    public float minTimeBetweenEnemies = 0.8f;
    public float maxTimeBetweenEnemies = 3.5f;
    private float timeBetweenEnemies = 0.0f;
    private int baseMobCount;
    private int advancedMobCount;
    public int baseEnemiesPerWave = 10;
    public int waveMultiplier = 2;

    private int waveNumber = 0;
    private bool spawningWave = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.currentRound = 0;
        towers = GameObject.FindGameObjectsWithTag("Tower");
        StartWaves();
        Debug.Log("Started code");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnWave() {
        if (spawningWave) {
            return;
        }

        spawningWave = true;
        waveNumber++;
        GameManager.instance.currentRound = waveNumber;
        baseMobCount = baseEnemiesPerWave + ((waveNumber - 1) * waveMultiplier);

        Debug.Log($"Starting wave {waveNumber}...");

        StartCoroutine(SpawnEnemies());       
    }

    private IEnumerator SpawnEnemies () {
        for (int i = 0; i < baseMobCount; i++) {
            // reset the path of the enemy
            int nextPoint = 0;

            List<GameObject> currentWaveEnemies = GetEnemiesInWave(GameManager.instance.currentRound);
            GameObject enemyPrefab = currentWaveEnemies[UnityEngine.Random.Range(0, currentWaveEnemies.Count)];

            // store a list of spawnPoints and convert them from GameObject[] to Transform[]
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
            List<Transform> spawnPointTransforms = spawnPoints.Select(spawn => spawn.transform).ToList();

            // Pick a random spawnPoint from the list we made
            Transform spawnPoint = spawnPointTransforms[UnityEngine.Random.Range(0, spawnPointTransforms.Count)];

            // Instantiate the enemy at the spawnpoint
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // If we spawned on a point that is not on the path, change the path of the enemy
            if (spawnPoint.name == "Spawn2") nextPoint = 2;

            enemy.GetComponent<EnemyBasicController>().WaypointIndex = nextPoint;

            // Wait a small, random amount of time before spawning the next enemy
            timeBetweenEnemies = UnityEngine.Random.Range(minTimeBetweenEnemies, maxTimeBetweenEnemies);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
        
        Debug.Log($"Wave {waveNumber} complete!");
        yield return new WaitForSeconds(timeBetweenWaves);

        spawningWave = false;
    }

    public void StartWaves() {
        InvokeRepeating("SpawnWave", 0.0f, timeBetweenWaves);
    }

    public void StopWaves() {
        CancelInvoke("SpawnWave");
    }
    
    private List<GameObject> GetEnemiesInWave(int waveNumber) {
        List<GameObject> currentWaveEnemies = enemyPrefabs.FindAll(enemyPrefab => enemyPrefab.GetComponent<Enemy>().enemyData.round <= waveNumber);
        return currentWaveEnemies;
    }
}
