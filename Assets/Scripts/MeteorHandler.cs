using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MeteorHandler : MonoBehaviour
{
    float time = 0;

    public float speed = 10f;
    public float amountPerSecond = 10;

    public GameObject meteorPrefab;
    public GameObject meteorTarget;
    public GameObject planet;

    private GameObject newMeteor;
    public List<GameObject> meteorPrefabs = new List<GameObject>();

    private GameObject newTarget;
    public List<GameObject> targetPrefabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= (1f / amountPerSecond))
        {
            spawnNewMeteor();
            time = 0;
        }

        for (int i = 0; i < meteorPrefabs.Count; i++)
        {
            if (meteorPrefabs[i] != null)
            {
                Transform pos = meteorPrefabs[i].transform;
                //newMeteorPrefabs[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

    void spawnNewMeteor()
    {
        Vector3 targetPos = Random.onUnitSphere * 10f;
        Vector3 spawnPos = targetPos * 3;
        newTarget = Instantiate(meteorTarget, targetPos, Quaternion.identity);
        newMeteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        //newMeteor.transform.LookAt(planet.transform);

        newTarget.transform.LookAt(planet.transform);
        targetPrefabs.Add(newTarget);
        meteorPrefabs.Add(newMeteor);
    }
}