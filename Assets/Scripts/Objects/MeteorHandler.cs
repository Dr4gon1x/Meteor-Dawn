using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeteorHandler : MonoBehaviour
{
    // ----------------------------------------------------------------

    public float speed = 10f;
    public float spawnDistance = 30f;
    public float amountPerSecond = 5f;
    public float despawnTime = 1f;
    public float targetScaleOnImpact = 1.5f;
    public float distanceForTargetActivate = 30f;
    public float spawnScaleSpeed = 2f;

    public GameObject meteorPrefab;
    public GameObject meteorTarget;

    [SerializeField] VisualEffect Impact;
    public GameObject planet;

    // ----------------------------------------------------------------

    private float time = 0;
    private float planetRadius;
    private float targetScaleIncrease;
    private float defaultScale;

    private GameObject newMeteor;
    public List<GameObject> meteorPrefabs = new List<GameObject>();

    private GameObject newTarget;
    public List<GameObject> targetPrefabs = new List<GameObject>();

    public List<GameObject> deadPrefabs = new List<GameObject>();
    private List<float> lifeTime = new List<float>();

    // ----------------------------------------------------------------

    void Start()
    {
        planetRadius = planet.transform.localScale.x / 2f;
        float distanceToPlanet = distanceForTargetActivate - planetRadius;
        targetScaleIncrease = (meteorTarget.transform.localScale.x * targetScaleOnImpact) / (distanceToPlanet / speed);

        defaultScale = meteorPrefab.transform.localScale.x;
    }

    void Update()
    {
        // Spawns new meteor
        time += Time.deltaTime;
        if (time >= (1f / amountPerSecond))
        {
            for (int i = 0; i < Mathf.Round(time / (1f / amountPerSecond)); i++)
            {
                spawnNewMeteor();
            }

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
        newMeteor.transform.localScale = new Vector3(0, 0, 0);
        newMeteor.gameObject.tag="Meteor";

        newTarget.transform.LookAt(planet.transform);
        newTarget.transform.localScale = new Vector3(0, 0, 0.1f);
        targetPrefabs.Add(newTarget);
        meteorPrefabs.Add(newMeteor);
    }

    // Opdatere meteor + meteor target
    void meteorTick()
    {
        for (int i = 0; i < meteorPrefabs.Count; i++)
        {
            if (meteorPrefabs[i] != null)
            {
            Transform pos = meteorPrefabs[i].transform;
            pos.position = Vector3.MoveTowards(pos.position, planet.transform.position, speed * Time.deltaTime);
            float dist = Vector3.Distance(planet.transform.position, meteorPrefabs[i].transform.position);

            if (dist <= distanceForTargetActivate)
            {
                targetPrefabs[i].transform.localScale = new Vector3(targetPrefabs[i].transform.localScale.x + (targetScaleIncrease * Time.deltaTime), targetPrefabs[i].transform.localScale.y + (targetScaleIncrease * Time.deltaTime), 0.1f);
            }

            if (meteorPrefabs[i].transform.localScale.x < defaultScale)
            {
                float scaleIncrease = defaultScale * spawnScaleSpeed * Time.deltaTime;
                meteorPrefabs[i].transform.localScale += new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);

                if (meteorPrefabs[i].transform.localScale.x > defaultScale)
                {
                meteorPrefabs[i].transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
                }
            }
            }
            else
            {
            Destroy(targetPrefabs[i]);

            VisualEffect impact = Instantiate(Impact, targetPrefabs[i].transform.position * 0.9975f, targetPrefabs[i].transform.rotation);
            impact.GetComponent<VisualEffect>();
            impact.Play();
            Destroy(impact.gameObject, 0.7f);
            targetPrefabs.Remove(targetPrefabs[i]);
            meteorPrefabs.Remove(meteorPrefabs[i]);
            }
        }
    }

    // opdatere impact efter meteor har ramt planeten
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