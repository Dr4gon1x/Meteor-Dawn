using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Vector3 randomPoint;
    private float delay = 1.5f; // Delay in seconds
    public GameObject Sten;

    private int amount_of_Sten = 15; // Amount of Sten to spawn
    private float spawnRadius = 2f; // Distance from the planet's surface for spawning Sten
    private float planetRadius = 5f; // Radius of the planet
    private float minDistanceBetweenSten = 2.5f; // Minimum distance between Sten to prevent overlap
    private float maxDistanceFromPlayer = 1.5f; // Maximum distance from the player to spawn Sten
    private float lifetime = 5f;

    private List<Vector3> spawnedPositions = new List<Vector3>(); // List to store spawned positions
    public List<GameObject> spawnedSten = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveObstacleWithDelay());
        StartCoroutine(DestoryObstacle());
    }

    IEnumerator DestoryObstacle()
    {
        while (true)
        {
            // Decrease the lifetime if there are more than 20 gameobjects
            if (spawnedSten.Count > 15)
            {
                // Sort the spawnedSten based on their distance from the player in descending order
                spawnedSten.Sort((a, b) => Vector3.Distance(b.transform.position, transform.position).CompareTo(Vector3.Distance(a.transform.position, transform.position)));

                // Remove the 5 farthest objects
                for (int i = 0; i < 5 && spawnedSten.Count > 0; i++)
                {
                    GameObject farthestTest = spawnedSten[0];
                    spawnedSten.RemoveAt(0);
                    Destroy(farthestTest);
                }
            }

            yield return new WaitForSeconds(lifetime);

            if (spawnedSten.Count > 0)
            {
                GameObject firstTest = spawnedSten[0];
                spawnedSten.Remove(firstTest);
                Destroy(firstTest);
            }
            else
            {
                Debug.LogError("No Obstacle to Destroy");
            }

            if (spawnedSten.Count == 0 && spawnedPositions.Count != 0)
            {
                GameObject firstTest = spawnedSten[0];
                spawnedSten.Remove(firstTest);
                Destroy(firstTest);
            }
        }
    }

    IEnumerator MoveObstacleWithDelay()
    {
        int StenSpawned = 0; // Counter for spawned Sten

        while (StenSpawned < amount_of_Sten) // Check if we have reached the desired number of Sten
        {
            // Generate a valid random position on the sphere surface
            Vector3 newPosition = GenerateValidPosition();

            if (newPosition != Vector3.zero) // Check if a valid position was found
            {
                // Calculate the rotation to align the Test's "up" direction with the surface normal
                Quaternion surfaceAlignment = Quaternion.LookRotation(newPosition.normalized, Vector3.up);

                // Spawn the Test at the new position with the calculated alignment
                GameObject newTest = Instantiate(Sten, newPosition, surfaceAlignment);

                // Add the new position to the list of spawned positions
                spawnedPositions.Add(newPosition);

                spawnedSten.Add(newTest);

                /* Debug.Log(spawnedSten.Count); */

                // Increment the counter
                StenSpawned++;

                // when the StenSpawned is equal to the amount_of_Sten, reset the counter and add a delay
                if (StenSpawned == amount_of_Sten)
                {
                    int BigDelay = 15;
                    StenSpawned = 0;
                    yield return new WaitForSeconds(BigDelay);
                }

            }

            // Wait for the specified delay before attempting to spawn the next Test
            yield return new WaitForSeconds(delay);
        }
    }

    Vector3 GenerateValidPosition()
    {
        int maxAttempts = 100; // Limit the number of attempts to find a valid position
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random point on the surface of a sphere with radius = planetRadius + spawnRadius
            Vector3 candidatePosition = Random.onUnitSphere * (planetRadius + spawnRadius);

            // Check if the candidate position is far enough from all existing Sten
            if (IsPositionValid(candidatePosition) && IsPositionValidForPlayer(candidatePosition))
            {
                return candidatePosition;
            }
        }
        // Return Vector3.zero if no valid position is found within the max attempts
        return Vector3.zero;
    }

    bool IsPositionValid(Vector3 position)
    {
        // Check the distance between the new position and all previously spawned positions
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistanceBetweenSten)
            {
                return false; // Position is too close to another Test
            }
        }
        return true; // Position is valid
    }

    bool IsPositionValidForPlayer(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) >= maxDistanceFromPlayer;
    }
}
