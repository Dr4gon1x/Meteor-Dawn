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
    public GameObject target;

    private GameObject newMeteor;
    public List<GameObject> newMeteorPrefabs = new List<GameObject>();

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

        for (int i = 0; i < newMeteorPrefabs.Count; i++)
        {
            if (newMeteorPrefabs[i] != null)
            {
                Transform pos = newMeteorPrefabs[i].transform;
                //newMeteorPrefabs[i].transform.Translate(Vector3.forward * speed * Time.deltaTime);
                pos.position = Vector3.MoveTowards(pos.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                newMeteorPrefabs.Remove(newMeteorPrefabs[i]);
            }
        }
    }

    void spawnNewMeteor()
    {
        Vector3 pos = Random.onUnitSphere * 30f;
        newMeteor = Instantiate(meteorPrefab, pos, Quaternion.identity);
        //newMeteor.transform.LookAt(target.transform);

        newMeteorPrefabs.Add(newMeteor);
    }
}