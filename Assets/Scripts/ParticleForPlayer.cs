using UnityEngine;

public class ParticleFlip : MonoBehaviour
{
    public ParticleSystem Fire; // Assign your ParticleSystem here
    public Transform player;              // Reference to the player object
    public float flipMultiplier = 1.0f;   // Control how strongly particles flip in the reverse direction

    private Vector3 lastPosition;

    void Start()
    {
        // Initialize the player's last position
        lastPosition = player.position;
    }

    void Update()
    {
        // Calculate the player's movement direction
        Vector3 playerMovementDirection = player.position - lastPosition;

        // Reverse the movement direction and normalize it for particle flipping
        Vector3 reversedDirection = -playerMovementDirection.normalized * flipMultiplier;

        // Set the velocity over lifetime for particle system to flip particles
        var velocityOverLifetime = Fire.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.x = reversedDirection.x;
        velocityOverLifetime.y = reversedDirection.y;
        velocityOverLifetime.z = reversedDirection.z;

        // Update the last position of the player for the next frame
        lastPosition = player.position;
    }
}
