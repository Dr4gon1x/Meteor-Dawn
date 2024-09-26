using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Vector3 randomPoint;
    public float delay = 1.0f; // Delay in seconds
    public GameObject Test;

    public int amount_of_Tests = 5; // Amount of Tests to spawn
    public float spawnRadius = 6f; // Distance from the planet's surface for spawning Tests
    public float planetRadius = 5f; // Radius of the planet
    public float minDistanceBetweenTests = 0.5f; // Minimum distance between Tests to prevent overlap

    private List<Vector3> spawnedPositions = new List<Vector3>(); // List to store spawned positions

    //private List <GameObject> Test_location = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveObstacleWithDelay());
    
    }

    IEnumerator MoveObstacleWithDelay()
    {
        int TestsSpawned = 0; // Counter for spawned Tests

        while (TestsSpawned < amount_of_Tests) // Check if we have reached the desired number of Tests
        {
            // Generate a valid random position on the sphere surface
            Vector3 newPosition = GenerateValidPosition();

            if (newPosition != Vector3.zero) // Check if a valid position was found
            {
                // Calculate the rotation to align the Test's "up" direction with the surface normal
                Quaternion surfaceAlignment = Quaternion.LookRotation(newPosition.normalized, Vector3.up);

                // Spawn the Test at the new position with the calculated alignment
                Instantiate(Test, newPosition, surfaceAlignment);

                // Add the new position to the list of spawned positions
                spawnedPositions.Add(newPosition);

                // Increment the counter
                TestsSpawned++;
            }
            

            // Wait for the specified delay before spawning the next Test
            yield return new WaitForSeconds(delay);
        }
    }

    Vector3 GenerateValidPosition()
    {
        int maxAttempts = 100; // Limit the number of attempts to find a valid position
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random point on the surface of a sphere with radius = planetRadius + spawnRadius
            Vector3 candidatePosition = Random.onUnitSphere * (planetRadius+ spawnRadius);

            // Check if the candidate position is far enough from all existing Tests
            if (IsPositionValid(candidatePosition))
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
            if (Vector3.Distance(position, spawnedPosition) < minDistanceBetweenTests)
            {
                return false; // Position is too close to another Test
            }
        }
        return true; // Position is valid
    }
}
