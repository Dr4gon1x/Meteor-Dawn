using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MeteorHandler : MonoBehaviour
{

    int childCount;
    float time = 0;

    public float speed = 10f;
    public GameObject meteorPrefab;
    public GameObject target;

    private GameObject newMeteor;
    public List<GameObject> newMeteorPrefabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        childCount = this.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
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
        int randomTal = Random.Range(0, childCount);
        Transform randomChild = this.transform.GetChild(randomTal);

        newMeteor = Instantiate(meteorPrefab, randomChild);
        randomChild.transform.DetachChildren();
        newMeteor.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        //newMeteor.transform.LookAt(target.transform);

        newMeteorPrefabs.Add(newMeteor);
    }
}