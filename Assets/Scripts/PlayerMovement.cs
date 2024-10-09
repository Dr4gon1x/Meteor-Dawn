using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float speed;
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
            SpeedFix();
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    void SpeedFix()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            speed = (float) (Mathf.Sqrt(2) / 2) * baseSpeed;
        }
        else
        {
            speed = baseSpeed;
        }

        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) 
        {
            speed = baseSpeed;
        }
    }
}
