using UnityEngine;
using UnityEngine.VFX;

public class VFXParticleFlip : MonoBehaviour
{
    public VisualEffect Player_Partical_v1;  // Assign the Visual Effect Graph here
    public Transform player;       // Reference to the player object
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

        // Reverse the movement direction and multiply by the flip multiplier
        Vector3 reversedDirection = -playerMovementDirection * flipMultiplier;

        // Pass the reversed movement to the VFX Graph
        Player_Partical_v1.SetVector3("PlayerReverseDirection", reversedDirection);

        // Update the last position of the player for the next frame
        lastPosition = player.position;
    }
}
