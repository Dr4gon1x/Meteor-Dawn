using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public FauxGravityAdd attractor;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        // Enable interpolation for smoother movement and rotation
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        attractor.Attract(rb);   
    }
}
