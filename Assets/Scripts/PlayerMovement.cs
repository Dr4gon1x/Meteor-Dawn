using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float speed;
    private Rigidbody rb;

    bool w; bool s; bool a; bool d;

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

        keysUpdate();

        if (w)
        {
            movement += transform.forward;
        }
        if (s)
        {
            movement -= transform.forward;
        }
        if (a)
        {
            movement -= transform.right;
        }
        if (d)
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
        keysUpdate();

        if ((w || s) && (a || d))
        {
            speed = (float) (Mathf.Sqrt(2) / 2) * baseSpeed;
        }
        else
        {
            speed = baseSpeed;
        }

        if ((w && s) || (a && d)) 
        {
            speed = baseSpeed;
        }
    }

    void keysUpdate()
    {
        w = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        s = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        a = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        d = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }
}
