using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movement_1();
    }

    void movement_1()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += transform.right;
        }

        // Move using Rigidbody's MovePosition
        if (movement != Vector3.zero)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }

    }
    
    // Check if the player collides with an object tagged "Sten"
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sten")
        {
            Debug.LogError("Sten is moving");
        }
    }

    
}
