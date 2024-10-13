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
    public float distanceBetweenObjects = 6.5f;
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

    // ----------------------------------------------------------------

    private float spawnDistanceFromPlanet;
    private float prefabWidth;
    private float time = 0;
    private bool obstacleIncrease = true;

    // ----------------------------------------------------------------

    private GameObject newObstacle;
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    public List<GameObject> dyingPrefabs = new List<GameObject>();
    private List<float> spawnOffset = new List<float>();
    private List<float> deathOffset = new List<float>();

    // ----------------------------------------------------------------

    void Start()
    {
        prefabWidth = obstaclePrefab.transform.localScale.x;

        spawnDepth += 1f;

        spawnDistanceFromPlanet = (planet.transform.localScale.x + spawnHeightIncrease) / 2;

        for (int i = 0; i < startAmount; i++)
        {
            spawnObject();
            obstaclePrefabs[i].transform.position *= spawnOffset[i];
            spawnOffset[i] = 1f;
        }
    }

    void Update()
    {
        posUpdate();

        time += Time.deltaTime;
        if (time >= updateTime)
        {
            spawnUpdate();
            time = 0;
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
            }

        } else if (obstacleIncrease == false)
        {
            for (int i = 0; i < Mathf.Round(time / updateTime) * amountDecrease; i++)
            {
                int randomObject = Random.Range(0, obstaclePrefabs.Count - 1);
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

        if (dist < distanceToPlayer)
        {
            return false;
        }

        foreach (GameObject obstaclePrefab in obstaclePrefabs)
        {
            dist = Vector3.Distance(obstaclePrefab.transform.position, spawnPos);

            if (prefabWidth / 2 * 1.5f < dist && dist < distanceBetweenObjects)
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
}
