using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    public float timeBetweenWaves = 10f;
    public float timeBetweenEnemies = 1f;
    private int mobCount;
    public int baseEnemiesPerWave = 10;
    public int waveMultiplier = 2;

    private int waveNumber = 0;
    private bool spawningWave = false;

    // Start is called before the first frame update
    void Start()
    {
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
        mobCount = baseEnemiesPerWave + (waveNumber * waveMultiplier);

        Debug.Log($"Starting wave {waveNumber}...");

        StartCoroutine(SpawnEnemies());       
    }

    private IEnumerator SpawnEnemies () {
        for (int i = 0; i < mobCount; i++) {
            int nextPoint = 0;
            GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
            String enemyType = enemyPrefab.GetComponent<Enemy>().enemyData.type;
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
            List<Transform> spawnPointTransforms = new List<Transform>();
            foreach (GameObject spawn in spawnPoints) {
                spawnPointTransforms.Add(spawn.transform);
            }
            Transform spawnPoint = spawnPointTransforms[UnityEngine.Random.Range(0, spawnPointTransforms.Count)];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.transform.rotation);
            if (spawnPoint.name == "Spawn2") {
                nextPoint = 2;
            }
            enemy.GetComponent<EnemyBasicController>().WaypointIndex = nextPoint;

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
}
