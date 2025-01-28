using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private float spawnBuffer = 10f;
    [SerializeField] private float safeZoneRadius = 10f;

    [Header("Meteor Spawning")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private int initialMeteors = 5;
    [SerializeField] private int maxMeteors = 10;

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 5;

    public static Vector2 MapSize { get; private set; }

    private List<GameObject> activeMeteors = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Transform player;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera is missing!");
            return;
        }

        float mapWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;
        float mapHeight = mainCamera.orthographicSize * 2;

        MapSize = new Vector2(mapWidth, mapHeight);

        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }
    }

    private void Start()
    {
        SpawnInitialMeteors();
        SpawnEnemies(maxEnemies);
    }

    private void Update()
    {
        if (activeMeteors.Count < maxMeteors)
        {
            SpawnMeteorOutsideView();
        }

        if (activeEnemies.Count < maxEnemies)
        {
            SpawnEnemies(maxEnemies - activeEnemies.Count);
        }
    }

    private void SpawnInitialMeteors()
    {
        for (int i = 0; i < initialMeteors; i++)
        {
            Vector3 spawnPosition;
            do
            {
                spawnPosition = GetRandomPositionWithinView();
            } while (IsWithinSafeZone(spawnPosition));

            SpawnMeteorAtPosition(spawnPosition);
        }
    }

    private void SpawnMeteorOutsideView()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = GetRandomPositionOutsideView();
        } while (IsWithinSafeZone(spawnPosition));

        Vector3 targetPosition = GetRandomPositionWithinView();
        SpawnMeteorAtPosition(spawnPosition, (targetPosition - spawnPosition).normalized);
    }

    private void SpawnMeteorAtPosition(Vector3 position, Vector3? direction = null)
    {
        GameObject meteor = Instantiate(meteorPrefab, position, Quaternion.identity);
        activeMeteors.Add(meteor);

        Meteor1 meteorScript = meteor.GetComponent<Meteor1>();
        if (meteorScript != null)
        {
            meteorScript.OnDestroyAction = () => activeMeteors.Remove(meteor);
            if (direction.HasValue)
            {
                meteorScript.SetDirection(direction.Value);
            }
        }
    }

    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition;
            do
            {
                spawnPosition = GetRandomPositionAtMapEdgeWithOffset();
            } while (IsWithinSafeZone(spawnPosition));

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.OnDestroyAction = () => activeEnemies.Remove(enemy);
            }
        }
    }

    private Vector3 GetRandomPositionWithinView()
    {
        float randomX = Random.Range(-MapSize.x / 2, MapSize.x / 2);
        float randomY = Random.Range(-MapSize.y / 2, MapSize.y / 2);
        return new Vector3(randomX, randomY, 0);
    }

    private Vector3 GetRandomPositionOutsideView()
    {
        float randomX = Random.Range(-spawnBuffer, MapSize.x + spawnBuffer);
        float randomY = Random.Range(-spawnBuffer, MapSize.y + spawnBuffer);

        if (randomX >= -MapSize.x / 2 && randomX <= MapSize.x / 2 &&
            randomY >= -MapSize.y / 2 && randomY <= MapSize.y / 2)
        {
            if (Random.value > 0.5f)
            {
                randomX = Random.value > 0.5f ? -spawnBuffer : MapSize.x + spawnBuffer;
            }
            else
            {
                randomY = Random.value > 0.5f ? -spawnBuffer : MapSize.y + spawnBuffer;
            }
        }

        return new Vector3(randomX - MapSize.x / 2, randomY - MapSize.y / 2, 0);
    }

    private Vector3 GetRandomPositionAtMapEdgeWithOffset()
    {
        float randomX = 0;
        float randomY = 0;

        if (Random.value > 0.5f)
        {
            randomX = Random.value > 0.5f ? -MapSize.x / 2 - spawnBuffer : MapSize.x / 2 + spawnBuffer;
            randomY = Random.Range(-MapSize.y / 2, MapSize.y / 2);
        }
        else
        {
            randomY = Random.value > 0.5f ? -MapSize.y / 2 - spawnBuffer : MapSize.y / 2 + spawnBuffer;
            randomX = Random.Range(-MapSize.x / 2, MapSize.x / 2);
        }

        return new Vector3(randomX, randomY, 0);
    }

    private bool IsWithinSafeZone(Vector3 position)
    {
        if (player == null) return false;

        float distance = Vector3.Distance(position, player.position);
        return distance < safeZoneRadius;
    }
}