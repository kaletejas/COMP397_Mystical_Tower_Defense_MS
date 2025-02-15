/*
    Author: Maxwel Santana
    Student Number: 301294337
    File: SpawnEnemy
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2;
    public int maxEnemies = 20;
}
*/

public class SpawnEnemy : MonoBehaviour
{

    public GameObject[] waypoints;
    public GameObject testEnemyPrefab;

    public Wave[] waves;
    public int timeBetweenWaves = 5;

    private GameManager gameManager;

    private float lastSpawnTime;
    private int enemiesSpawned = 0;

    public bool StartWave { get; set; }

    // Use this for initialization
    void Start()
    {
        lastSpawnTime = Time.time;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartWave) { return; }
        int currentWave = gameManager.Wave;
        if (currentWave < waves.Length)
        {
            float timeInterval = Time.time - lastSpawnTime;
            float spawnInterval = waves[currentWave].spawnInterval;
            if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) ||
                 timeInterval > spawnInterval) &&
                enemiesSpawned < waves[currentWave].maxEnemies)
            {
                lastSpawnTime = Time.time;
                GameObject newEnemy = Instantiate(waves[currentWave].enemyPrefab, waypoints[0].transform.position, Quaternion.identity);
                newEnemy.GetComponent<MoveEnemy>().waypoints = waypoints;
                enemiesSpawned++;
            }

            if (enemiesSpawned == waves[currentWave].maxEnemies &&
                GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                gameManager.Wave++;
                //gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
                LevelManager.instance.currency.AddCurrency(1);
                enemiesSpawned = 0;
                lastSpawnTime = Time.time;
            }
        }
        else
        {
            LevelManager.instance.GameOver();
        }
    }

}
