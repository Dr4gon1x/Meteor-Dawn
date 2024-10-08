using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawn_VFX : MonoBehaviour
{
    public GameObject vfx;
    public Transform startPoint;
    public Transform endPoint;
    public float spawnRange = 600f; // Range around the start point to spawn meteors
    public float moveSpeed = 100f; // Speed at which meteors move
    public GameObject impactPregab;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        SpawnMeteors(15);
        rb = GetComponent<Rigidbody>();
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
            StartCoroutine(MoveMeteor(vfxObj, startPos, endPos));
        }
    }

    IEnumerator MoveMeteor(GameObject meteor, Vector3 startPos, Vector3 endPos)
    {
        while (true)
        {
            float journeyLength = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;

            while (Vector3.Distance(meteor.transform.position, endPos) > 0.1f)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;
                meteor.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
                yield return null;
            }

            meteor.transform.position = startPos;
            RotateTo(meteor, endPos);
        }
    }
    void FixedUpdate()
    {
        if (moveSpeed != 0 && rb != null)
        {
            rb.position += transform.forward * (moveSpeed * Time.deltaTime);
        }
    }
}

