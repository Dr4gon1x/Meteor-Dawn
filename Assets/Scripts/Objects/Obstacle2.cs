using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle2 : MonoBehaviour
{
    // ----------------------------------------------------------------

    public float updateTime = 3f;
    public int amountIncrease = 3;
    public int amountDecrease = 4;
    public float distanceBetweenObjects = 2.5f;
    public float distanceToPlayer = 4f;
    public float spawnHeightIncrease = 1f;
    public int maxObjects = 15;
    public int minObjects = 10;
    public int startAmount = 5;
    public float spawnDepth = 0.2f;
    public float rotateSpeed = 360f;
    public float moveSpeed = 1f;

    public GameObject planet;
    public GameObject player;
    public GameObject obstaclePrefab;

    public int objectIncrease = 3;
    public float scaleIncrease = 0.1f;
    public float maxScale = 1.5f;
    public float timeBeforeStart = 5f;

    // ----------------------------------------------------------------

    private float spawnDistanceFromPlanet;
    private float time = 0;
    private float startCooldown = 0;
    private float objectScaleMultiplier = 1f;
    private bool obstacleIncrease = true;
    private bool startAmountSpawned = false;
    private int obstacleThredshold = 100;

    // ----------------------------------------------------------------

    private GameObject newObstacle;
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    public List<GameObject> dyingPrefabs = new List<GameObject>();
    private List<float> spawnOffset = new List<float>();
    private List<float> deathOffset = new List<float>();

    // ----------------------------------------------------------------

    void Start()
    {
        spawnDepth += 1f;

        spawnDistanceFromPlanet = (planet.transform.localScale.x + spawnHeightIncrease) / 2;
    }

    void Update()
    {
        if (startCooldown < timeBeforeStart)
        {
            startCooldown += Time.deltaTime;
        } else
        {
            if (startAmountSpawned == false)
            {
                for (int i = 0; i < startAmount; i++)
                {
                    spawnObject();
                }

                startAmountSpawned = true;
            }

            time += Time.deltaTime;
            if (time >= updateTime && obstaclePrefabs.Count < obstacleThredshold)
            {
                spawnUpdate();
                time = 0;
            }

            posUpdate();
        }
    }

    // ----------------------------------------------------------------
    // Opdatere obstacle spawns
    void spawnUpdate()
    {
        if (obstacleIncrease == true)
        {
            for (int i = 0; i < Mathf.Round(time / updateTime) * amountIncrease; i++)
            {
                spawnObject();
            }

            if (obstaclePrefabs.Count >= maxObjects)
            {
                obstacleIncrease = false;
                difficultyHandler();
            }

        } else if (obstacleIncrease == false)
        {
            for (int i = 0; i < Mathf.Round(time / updateTime) * amountDecrease; i++)
            {
                int randomObject = Random.Range(0, obstaclePrefabs.Count);
                dyingPrefabs.Add(obstaclePrefabs[randomObject]);
                deathOffset.Add(1f);
                obstaclePrefabs.RemoveAt(randomObject);;
                spawnOffset.RemoveAt(randomObject);;


                if (obstaclePrefabs.Count == 0)
                {
                    break;
                }
            }

            if (obstaclePrefabs.Count <= minObjects)
            {
                obstacleIncrease = true;
                difficultyHandler();
            }
        }
    }

    // ----------------------------------------------------------------
    // Spawner objekt på planeten
    private void spawnObject()
    {
        bool spawnSuccess = false;
        while (spawnSuccess != true)
        {
            Vector3 spawnPos = Random.onUnitSphere * spawnDistanceFromPlanet;
            if (spawnAttemptCheck(spawnPos))
            {
                newObstacle = Instantiate(obstaclePrefab, spawnPos / spawnDepth, Quaternion.identity);

                newObstacle.transform.LookAt(planet.transform);
                newObstacle.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
                newObstacle.transform.localScale *= objectScaleMultiplier; // Increases scale

                obstaclePrefabs.Add(newObstacle);
                spawnOffset.Add(spawnDepth);

                spawnSuccess = true;
            }
        }
    }

    // ----------------------------------------------------------------
    // Checker om man kan spawne en sten, på den givne spawn position
    bool spawnAttemptCheck(Vector3 spawnPos)
    {
        Vector3 playerPos = player.transform.position;

        float dist = Vector3.Distance(playerPos, spawnPos);
        float currentWidth = obstaclePrefab.transform.localScale.x * objectScaleMultiplier;

        if (dist < distanceToPlayer)
        {
            return false;
        }

        foreach (GameObject obstacle in obstaclePrefabs)
        {
            dist = Vector3.Distance(obstacle.transform.position, planet.transform.position);

            Vector3 pos; // This if statement changes the gameobjects pos to its spawnpos, if it's underground due to just having spawned.
            if (dist < spawnDistanceFromPlanet)
            {
                float multiplyPos = spawnDistanceFromPlanet / dist;
                pos = obstacle.transform.position * multiplyPos;
            } else
            {
                pos = obstacle.transform.position;
            }

            dist = Vector3.Distance(pos, spawnPos);
            float obstacleWidth = obstacle.transform.localScale.x;

            if (obstacleWidth * 0.75f < dist && dist < (currentWidth + obstacleWidth) / 2 + distanceBetweenObjects)
            {
                return false;
            }
        }

        return true;
    }

    // ----------------------------------------------------------------
    // Opdatere obstacle positionen
    private void posUpdate()
    {
        for (int i = 0; i < obstaclePrefabs.Count; i++)
        {
            Vector3 obstaclePos = obstaclePrefabs[i].transform.position;

            if (spawnOffset[i] > 1f)
            {
                obstaclePos *= spawnOffset[i];

                spawnOffset[i] -= moveSpeed * Time.deltaTime;

                if (i % 2 == 0)
                {
                    obstaclePrefabs[i].transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                } else
                {
                    obstaclePrefabs[i].transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
                }

                if (spawnOffset[i] < 1f)
                {
                    spawnOffset[i] = 1f;
                }

                obstaclePos /= spawnOffset[i];
            }

            obstaclePrefabs[i].transform.position = obstaclePos;
        }

        for (int i = 0; i < dyingPrefabs.Count; i++)
        {
            if (deathOffset[i] < spawnDepth)
            {
                dyingPrefabs[i].transform.position *= deathOffset[i];

                deathOffset[i] += moveSpeed * Time.deltaTime;

                if (i % 2 == 0)
                {
                    dyingPrefabs[i].transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                }
                else
                {
                    dyingPrefabs[i].transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
                }

                dyingPrefabs[i].transform.position /= deathOffset[i];

            } else
            {
                Destroy(dyingPrefabs[i]);
                dyingPrefabs.RemoveAt(i);
                deathOffset.RemoveAt(i);
            }
        }
    }

    // ----------------------------------------------------------------
    /* Increases obstacle amount and size. When maxObjects is reached, it will increase maxObject. 
     * When minObjects is reached, it will be increase with half as much as maxObjects is increased. 
     * If increase is odd number like 3, then it increase with 1 with a 50% chance of increasing by 2.
     * When min is reached, size of prefabs is also increased, as well as distance to player. */

    void difficultyHandler()
    {
        if (obstaclePrefabs.Count >= maxObjects)
        {
            maxObjects += objectIncrease;
        }
        else if (obstaclePrefabs.Count <= minObjects)
        {
            minObjects += Mathf.RoundToInt(objectIncrease / 2);
            if (minObjects % 2 == 0 && Random.value < 0.5f)
            {
                minObjects += 1;
            }

            if (objectScaleMultiplier < maxScale)
            {
                distanceToPlayer /= objectScaleMultiplier;
                spawnHeightIncrease /= objectScaleMultiplier;
                spawnDepth /= objectScaleMultiplier;

                objectScaleMultiplier += scaleIncrease;

                if (objectScaleMultiplier > maxScale)
                {
                    objectScaleMultiplier = maxScale;
                }

                distanceToPlayer *= objectScaleMultiplier;
                spawnHeightIncrease *= objectScaleMultiplier;
                spawnDepth *= objectScaleMultiplier;
            }
        }
    }

    // ----------------------------------------------------------------
}
