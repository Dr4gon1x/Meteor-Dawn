using UnityEngine;
using UnityEngine.VFX;

public class VFXParticleFlip : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    public Transform playerTransform;
    public Transform Planet;
    public float rotationSpeed = 5f;

    void Update()
    {
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (movementDirection != Vector3.zero)
        {
            // Calculate the direction from the player to the center of the planet
            Vector3 planetCenterDirection = (playerTransform.position - Planet.position).normalized;

            // Calculate the right direction relative to the planet's surface
            Vector3 rightDirection = Vector3.Cross(planetCenterDirection, Vector3.up).normalized;

            // Calculate the forward direction relative to the planet's surface
            Vector3 forwardDirection = Vector3.Cross(rightDirection, planetCenterDirection).normalized;

            // Calculate the target rotation based on the movement direction and the planet's surface
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, planetCenterDirection);

            ParticleSystem.transform.rotation = Quaternion.Slerp(ParticleSystem.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
