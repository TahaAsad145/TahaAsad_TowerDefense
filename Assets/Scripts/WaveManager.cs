using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public int enemiesPerWave = 5;
    public float timeBetweenEnemies = 1f;
    public float timeBetweenWaves = 5f;

    public TextMeshProUGUI waveText;

    private int currentWave = 1;
    private int totalWaves;
    private bool isSpawning = false;

    void Start()
    {
        totalWaves = GetTotalWavesForCurrentLevel();
        UpdateWaveText();
        StartCoroutine(HandleAllWaves());
    }

    IEnumerator HandleAllWaves()
    {
        while (currentWave <= totalWaves)
        {
            yield return StartCoroutine(SpawnWave());
            currentWave++;
            UpdateWaveText();
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        LevelManager.instance.LevelWon();
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;
        Debug.Log($"Wave {currentWave} starting!");

        int enemiesThisWave = enemiesPerWave + currentWave * (IsLevel2() ? 2 : 1);

        for (int i = 0; i < enemiesThisWave; i++)
        {
            SpawnEnemy(currentWave);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        isSpawning = false;
    }

    void SpawnEnemy(int lvl)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.lvl = lvl;

            if (Random.value < 0.2f || lvl % 5 == 0)
            {
                health.isSpecial = true;
                health.goldValue = 50;

                Renderer rend = enemy.GetComponent<Renderer>();
                if (rend == null)
                    rend = enemy.GetComponentInChildren<Renderer>();

                if (rend != null)
                    rend.material.color = Color.blue;
            }
        }

        EnemyMovement move = enemy.GetComponent<EnemyMovement>();
        if (move != null)
        {
            if (SceneManager.GetActiveScene().name == "Level-1")
                move.waypoints = GetWayPointsInOrder();
            else
                move.waypoints = GetRandomPath();
        }
    }

    int GetTotalWavesForCurrentLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level-1")
            return 5; // Easier level
        else if (sceneName == "Level-2")
            return 10; // Harder level

        return 5; // Default fallback
    }

    bool IsLevel2()
    {
        return SceneManager.GetActiveScene().name == "Level-2";
    }

    Transform[] GetWayPointsInOrder()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("WayPoint");
        Transform[] ordered = new Transform[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
            ordered[i] = waypoints[i].transform;

        System.Array.Sort(ordered, (a, b) => a.name.CompareTo(b.name));
        return ordered;
    }

    Transform[] GetRandomPath()
    {
        string[] pathNames = { "PathA", "PathB" };
        string chosen = pathNames[Random.Range(0, pathNames.Length)];

        GameObject pathRoot = GameObject.Find(chosen);
        if (pathRoot == null)
        {
            Debug.LogWarning("Path not found: " + chosen);
            return new Transform[0];
        }

        Transform[] points = new Transform[pathRoot.transform.childCount];
        for (int i = 0; i < points.Length; i++)
            points[i] = pathRoot.transform.GetChild(i);

        return points;
    }

    void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {Mathf.Min(currentWave, totalWaves)} / {totalWaves}";
        }
    }
}
