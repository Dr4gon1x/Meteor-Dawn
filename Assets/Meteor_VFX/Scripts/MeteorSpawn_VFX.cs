using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn_VFX : MonoBehaviour
{
    public GameObject vfx;
    public Transform startPoint;
    public Transform endPoint;
    public float spawnRange = 700f; // Range around the start point to spawn meteors

    // Start is called before the first frame update
    void Start()
    {
        SpawnMeteors(50);
    }

    void RotateTo(GameObject obj, Vector3 destination)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    void SpawnMeteors(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var randomOffset = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange)
            );

            var startPos = startPoint.position + randomOffset;
            GameObject vfxObj = Instantiate(vfx, startPos, Quaternion.identity) as GameObject;

            var endPos = endPoint.position;

            RotateTo(vfxObj, endPos);
        }
    }
}
