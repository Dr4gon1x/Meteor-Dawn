using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle2 : MonoBehaviour
{
    // ----------------------------------------------------------------

    public float updateTime = 3;
    public int amountChangePerUpdate = 1;
    public float minDistanceBetweenObjects = 3;
    public int maxObjects = 20;
    public int minObjects = 15;

    public GameObject planet;
    public GameObject player;
    public GameObject obstaclePrefab;

    // ----------------------------------------------------------------

    private float minimumDistanceToPlayer;
    private float distanceBetweenNodes;
    private float prefabWidth;
    private float prefabHeight;
    private float time = 0;

    // ----------------------------------------------------------------

    private GameObject newObject;
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    private List<Vector3> nodes = new List<Vector3>();

    // ----------------------------------------------------------------

    void Start()
    {
        prefabWidth = obstaclePrefab.transform.localScale.x;
        prefabHeight = obstaclePrefab.transform.localScale.z;
    }

    void Update()
    {

    }

    // ----------------------------------------------------------------

    private void spawnObject()
    {
        
    }

    // ----------------------------------------------------------------
    bool spawnAttemptCheck(Vector3 spawnPos)
    {
        Vector3 playerPos = player.transform.position;

        float dist = Vector3.Distance(playerPos, spawnPos);

        if (dist < prefabWidth * 2f)
        {
            return false;
        }

        foreach (GameObject obstaclePrefab in obstaclePrefabs)
        {
            dist = Vector3.Distance(transform.position, spawnPos);

            if (dist <= prefabWidth)
            {
                return false;
            }

            if (dist > prefabWidth * 1.5f && dist < minDistanceBetweenObjects)
            {
                return false;
            }
        }

        return true;
    }

    // ----------------------------------------------------------------
}
