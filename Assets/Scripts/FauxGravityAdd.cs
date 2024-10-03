using UnityEngine;

public class FauxGravityAdd : MonoBehaviour
{
    public float gravity = -20f;  // Increased gravity strength for faster pull towards the object

    public void Attract(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.transform.up;

        // Apply gravity force
        body.AddForce(gravityUp * gravity);

        // Smooth the rotation to prevent jittering
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation, Time.deltaTime * 5f);
    }
}
