using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    private Vector3 randomPoint;
    public float delay = 1.0f; // Delay in seconds

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveObstacleWithDelay());
    }

    IEnumerator MoveObstacleWithDelay()
    {
        while (true)
        {
            randomPoint = Random.onUnitSphere * 10;
            transform.position = randomPoint;
            yield return new WaitForSeconds(delay);
        }
    }
}
