using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MeteorHandler : MonoBehaviour
{
    // ----------------------------------------------------------------

    public float speed = 10f;
    public float spawnDistance = 20f;
    public float amountPerSecond = 5f;
    public float despawnTime = 1f;

    public GameObject meteorPrefab;
    public GameObject meteorTarget;
    public GameObject planet;

    // ----------------------------------------------------------------

    private float time = 0;

    private GameObject newMeteor;
    public List<GameObject> meteorPrefabs = new List<GameObject>();

    private GameObject newTarget;
    public List<GameObject> targetPrefabs = new List<GameObject>();

    private GameObject deadMeteor;
    public List<GameObject> deadPrefabs = new List<GameObject>();

    // ----------------------------------------------------------------

    void Update()
    {
        time += Time.deltaTime;
        if (time >= (1f / amountPerSecond))
        {
            spawnNewMeteor();
            time = 0;
        }

        meteorTick();
    }

    void spawnNewMeteor()
    {
        // Give 
        Vector3 targetPos = Random.onUnitSphere * 10f;
        Vector3 spawnPos = targetPos * 3;

        // Spawn meteor and target
        newTarget = Instantiate(meteorTarget, targetPos, Quaternion.identity);
        newMeteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        newTarget.transform.LookAt(planet.transform);
        targetPrefabs.Add(newTarget);
        meteorPrefabs.Add(newMeteor);
    }

    void meteorTick()
    {
        for (int i = 0; i < meteorPrefabs.Count; i++)
        {
            if (meteorPrefabs[i] != null)
            {
                Transform pos = meteorPrefabs[i].transform;
                pos.position = Vector3.MoveTowards(pos.position, planet.transform.position, speed * Time.deltaTime);
            }
            else
            {
                Destroy(targetPrefabs[i]);
                targetPrefabs.Remove(targetPrefabs[i]);
                meteorPrefabs.Remove(meteorPrefabs[i]);
            }
        }
    }
}