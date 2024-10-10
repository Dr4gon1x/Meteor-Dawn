using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle2 : MonoBehaviour
{
    // ----------------------------------------------------------------

    public float updateTime = 3;
    public int amountIncrease = 3;
    public int amountDecrease = 4;
    public float distanceBetweenObjects = 6.5f;
    public float distanceToPlayer = 4;
    public float spawnRadiusIncrease = 1;
    public int maxObjects = 15;
    public int minObjects = 10;
    public int startAmount = 5;

    public GameObject planet;
    public GameObject player;
    public GameObject obstaclePrefab;

    // ----------------------------------------------------------------

    private float minimumDistanceToPlayer;
    private float spawnDistanceFromPlanet;
    private float prefabWidth;
    private float prefabHeight;
    private float time = 0;
    private bool obstacleIncrease = true;

    // ----------------------------------------------------------------

    private GameObject newObstacle;
    public List<GameObject> obstaclePrefabs = new List<GameObject>();

    // ----------------------------------------------------------------

    void Start()
    {
        prefabWidth = obstaclePrefab.transform.localScale.x;
        prefabHeight = obstaclePrefab.transform.localScale.z / 2;

        spawnDistanceFromPlanet = (planet.transform.localScale.x + spawnRadiusIncrease) / 2;

        for (int i = 0; i < startAmount; i++)
        {
            spawnObject();
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= updateTime)
        {
            spawnUpdate();
            time = 0;
        }
    }

    // ----------------------------------------------------------------

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
                Destroy(obstaclePrefabs[randomObject]);
                obstaclePrefabs.RemoveAt(randomObject);

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

    private void spawnObject()
    {
        bool spawnSuccess = false;
        while (spawnSuccess != true)
        {
            Vector3 spawnPos = Random.onUnitSphere * spawnDistanceFromPlanet;
            if (spawnAttemptCheck(spawnPos))
            {
                newObstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
                newObstacle.transform.LookAt(planet.transform);
                newObstacle.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
                obstaclePrefabs.Add(newObstacle);
                spawnSuccess = true;
            }
        }
    }

    // ----------------------------------------------------------------
    bool spawnAttemptCheck(Vector3 spawnPos)
    {
        Vector3 playerPos = player.transform.position;

        float dist = Vector3.Distance(playerPos, spawnPos);

        if (dist < distanceToPlayer)
        {
            return false;
        }
        
        for(int i = 0; i < obstaclePrefabs.Count; i++)
        {
            dist = Vector3.Distance(obstaclePrefabs[i].transform.position, spawnPos);

            if (prefabWidth / 2 * 1.5f < dist && dist < distanceBetweenObjects)
            {
                return false;
            }
        }

        return true;
    }

    // ----------------------------------------------------------------
}
