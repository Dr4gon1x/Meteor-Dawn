using Unity.VisualScripting;
using UnityEngine;

public class FauxGravityAdd : MonoBehaviour
{
    public float gravity = -10f;
    public void Attract(Rigidbody body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.transform.up;

        Debug.Log(gravityUp.magnitude);
        body.AddForce(gravityUp * gravity);

        body.rotation = Quaternion.FromToRotation(bodyUp,gravityUp) * body.rotation;
    }
}
