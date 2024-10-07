using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class MeteorHandler : MonoBehaviour
{
    // ----------------------------------------------------------------

    public float speed = 10f;
    public float spawnDistance = 30f;
    public float amountPerSecond = 5f;
    public float despawnTime = 1f;
    public float targetScaleOnImpact = 1.5f;

    public GameObject meteorPrefab;
    public GameObject meteorTarget;
    public GameObject deadMeteorPrefab;
    public GameObject planet;

    // ----------------------------------------------------------------

    private float time = 0;
    private float planetRadius;
    private float targetScaleIncrease;

    private GameObject newMeteor;
    public List<GameObject> meteorPrefabs = new List<GameObject>();

    private GameObject newTarget;
    public List<GameObject> targetPrefabs = new List<GameObject>();

    private GameObject deadMeteor;
    public List<GameObject> deadPrefabs = new List<GameObject>();
    private List<float> lifeTime = new List<float>();

    // ----------------------------------------------------------------

    void Start()
    {
        planetRadius = planet.transform.localScale.x / 2;

        float distanceToPlanet = spawnDistance - planetRadius;
        targetScaleIncrease = ((distanceToPlanet / speed) * meteorTarget.transform.localScale.x) * targetScaleOnImpact;
    }

    void Update()
    {
        // Spawns new meteor
        time += Time.deltaTime;
        if (time >= (1f / amountPerSecond))
        {
            spawnNewMeteor();
            time = 0;
        }

        // Updates meteor
        meteorTick();
        deadMeteorTick();
    }

    void spawnNewMeteor()
    {
        // Give spawn and target position
        Vector3 targetPos = Random.onUnitSphere * planetRadius;
        Vector3 spawnPos = targetPos * (spawnDistance / planetRadius);

        // Spawn meteor and target
        newTarget = Instantiate(meteorTarget, targetPos, Quaternion.identity);
        newMeteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        newMeteor.gameObject.tag="Meteor";

        newTarget.transform.LookAt(planet.transform);
        newTarget.transform.localScale = new Vector3(0, 0, 0.1f);
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
                targetPrefabs[i].transform.localScale = new Vector3(targetPrefabs[i].transform.localScale.x + (targetScaleIncrease * Time.deltaTime), targetPrefabs[i].transform.localScale.y + (targetScaleIncrease * Time.deltaTime), 0.1f);
            }
            else
            {
                Destroy(targetPrefabs[i]);

                deadMeteor = Instantiate(deadMeteorPrefab, targetPrefabs[i].transform.position * 0.9975f, targetPrefabs[i].transform.rotation);
                deadMeteor.transform.localScale *= targetScaleOnImpact;
                deadPrefabs.Add(deadMeteor);
                lifeTime.Add(despawnTime);

                targetPrefabs.Remove(targetPrefabs[i]);
                meteorPrefabs.Remove(meteorPrefabs[i]);
            }
        }
    }

    void deadMeteorTick()
    {
        for (int i = 0; i < deadPrefabs.Count; i++)
        {
            if (deadPrefabs[i] != null)
            {
                lifeTime[i] -= Time.deltaTime;
                if (lifeTime[i] <= 0)
                {
                    Destroy(deadPrefabs[i]);
                }
            } else
            {
                deadPrefabs.Remove(deadPrefabs[i]);
                lifeTime.Remove(lifeTime[i]);
            }
        }
    }
}