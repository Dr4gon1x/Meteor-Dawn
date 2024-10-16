using UnityEngine;
using UnityEngine.VFX;

public class PlayerPositionBinder : MonoBehaviour
{
    private VisualEffect visualEffect; // Reference to the VFX component

    void Start()
    {
        // Get the Visual Effect component attached to the GameObject
        visualEffect = GetComponent<VisualEffect>();

        // Check if the Visual Effect component is attached
        if (visualEffect == null)
        {
            Debug.LogError("VisualEffect component is missing from the Player GameObject.");
        }
    }

    void Update()
    {
        // Only update if the visualEffect is not null
        if (visualEffect != null)
        {
            // Update the PlayerPosition property in the VFX Graph
            visualEffect.SetVector3("PlayerPosition", transform.position);
            visualEffect.SetVector3("PlayerVelocity", GetComponent<Rigidbody>().velocity);
        }
    }
}