using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemySpawnerController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List of enemy prefabs to spawn from
    public List<Transform> spawnPoints; // List of spawn points
    public int enemiesPerWave = 5; // Number of enemies per wave
    public int spawnPointsPerWave = 2; // Number of spawn points to use per wave
    public float timeBetweenWaves = 5f; // Time between waves
    public GameObject summonEffectPrefab; // Summon effect prefab
    public float summonEffectDuration = 1f; // Duration of the summon effect animation
    public int totalWaves = 3; // Total number of waves (can be modified)
    public GameObject portal; // Portal GameObject to activate when waves are complete
    private int remainingEnemy;

    private int totalEnemies;
    private int waveNumber = 0; // Current wave number
    private int activeEnemies = 0; // Active enemy count
    private Coroutine spawnCoroutine; // Coroutine reference for spawning waves
    private BuffSelectionUI buffSelectionUI; // Reference to the buff selection UI

    void OnEnable()
    {
        // Start spawning waves when object is enabled
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnWaves());
        }
    }

    void OnDisable()
    {
        // Stop spawning waves when object is disabled
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    // private void Update()
    // {
    //     if (SceneManager.GetActiveScene().name == "Endless")
    //     {
    //         if (Damageable.defeatedEnemyCount == totalEnemies)
    //         {
    //             // ShowBuffSelection();
    //             ResetSpawner();
    //             Debug.Log("total: " + totalEnemies);
    //             Debug.Log("enemy per wave: " + totalEnemies);
    //         }
    //         // Debug.Log("Defeat: " + Damageable.defeatedEnemyCount);
    //         //  Debug.Log("enemy per wave: " +totalWaves*spawnPointsPerWave);
    //     }
    // }
    void Start()
    {
        // Disable the portal at the start of the game
        if (portal != null)
        {
            portal.SetActive(false); // Deactivate the portal initially
        }

        //remainingEnemy = totalWaves * spawnPointsPerWave;
    }

    public IEnumerator SpawnWaves()
    {
        totalEnemies += totalWaves * spawnPointsPerWave;
        while (waveNumber < totalWaves)
        {
            // Spawn enemies for the current wave
            List<Transform> selectedSpawnPoints = SelectRandomSpawnPoints(spawnPointsPerWave);
            for (int i = 0; i < selectedSpawnPoints.Count; i++)
            {
                for (int j = 0; j < enemiesPerWave; j++)
                {
                    StartCoroutine(SpawnEnemyWithEffect(selectedSpawnPoints[i]));

                    // Optionally, add a small delay between spawning each enemy to stagger them
                    yield return new WaitForSeconds(0.3f);
                }
            }

            // Wait until all enemies are defeated
            while (activeEnemies > 0)
            {
                yield return null; // Wait until activeEnemies becomes 0
            }

            // Wave completed, proceed to the next one
            waveNumber++;

            // Wait between waves
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        // If all waves are complete and all enemies are defeated, activate the portal and show buff selection
        if (waveNumber >= totalWaves && activeEnemies == 0)
        {
            ActivatePortal();
            ShowBuffSelection();
            Debug.Log(Damageable.defeatedEnemyCount + " " + totalEnemies);
            if (SceneManager.GetActiveScene().name == "Endless")
            {
                if (Damageable.defeatedEnemyCount == totalEnemies)
                {
                    // ShowBuffSelection();
                    ResetSpawner();
                    Debug.Log("total: " + totalEnemies);
                    Debug.Log("enemy per wave: " + totalEnemies);
                }
                // Debug.Log("Defeat: " + Damageable.defeatedEnemyCount);
                //  Debug.Log("enemy per wave: " +totalWaves*spawnPointsPerWave);
            }
            Debug.Log("Done wave");
            //ResetSpawner();
        }
    }

    IEnumerator SpawnEnemyWithEffect(Transform spawnPoint)
    {
        // Instantiate the summon effect
        GameObject summonEffect = Instantiate(summonEffectPrefab, spawnPoint.position, spawnPoint.rotation);

        // Wait for the summon effect duration
        yield return new WaitForSeconds(summonEffectDuration);

        // Destroy the summon effect
        Destroy(summonEffect);

        // Instantiate the enemy
        SpawnEnemy(spawnPoint);
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned to the spawner!");
            return;
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
        activeEnemies++;
        // Assign waypoints to the flying enemy
        FlyingEye flyingEye = spawnedEnemy.GetComponent<FlyingEye>();
        EnemyXPTracker enemyXPTracker = spawnedEnemy.GetComponent<EnemyXPTracker>();
        enemyXPTracker.SpawnEnemyWithXP();
        if (flyingEye != null)
        {
            flyingEye.waypoints = new List<Transform>
            {
                GameObject.Find("Waypoint1").transform,
                GameObject.Find("Waypoint2").transform,
                GameObject.Find("Waypoint3").transform,
                GameObject.Find("Waypoint4").transform
            };
        }

        // Add a collider and trigger event to detect when the enemy is destroyed
        EnemyTracker tracker = spawnedEnemy.AddComponent<EnemyTracker>();
        tracker.spawner = this;
    }

    public void HandleEnemyDeath()
    {
        activeEnemies--;
    }

    void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true); // Enable the portal when waves are complete
        }
    }

    void ShowBuffSelection()
    {
        buffSelectionUI = FindObjectOfType<BuffSelectionUI>(); // Find the BuffSelectionUI
        if (buffSelectionUI == null)
        {
            Debug.LogError("BuffSelectionUI not found!");
        }
        if (buffSelectionUI != null)
        {
            buffSelectionUI.ShowBuffChoices();
        }
    }

    List<Transform> SelectRandomSpawnPoints(int count)
    {
        List<Transform> selectedPoints = new List<Transform>();
        List<int> usedIndices = new List<int>();

        while (selectedPoints.Count < count)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                usedIndices.Add(randomIndex);
                selectedPoints.Add(spawnPoints[randomIndex]);
            }
        }

        return selectedPoints;
    }
    private GameObject player;
    private CharacterStat characterStat;
    public void ResetSpawner()
    {
        // waveNumber = 0; // Reset the wave number
        activeEnemies = 0; // Reset the active enemies count
        totalWaves++;
        waveNumber = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            characterStat = player.GetComponent<CharacterStat>();
        }
        characterStat.Shield = 37;
        // remainingEnemy = totalWaves * spawnPointsPerWave; // Reset remaining enemies
        // Optionally, restart the spawning process
        StartCoroutine(SpawnWaves());
    }
}

public class EnemyTracker : MonoBehaviour
{
    public EnemySpawnerController spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.HandleEnemyDeath();
        }
    }
}
