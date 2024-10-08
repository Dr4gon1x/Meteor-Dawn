using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMove_vfx : MonoBehaviour
{

    public float speed;
    public GameObject impactPregab;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(speed != 0 && rb != null)
        {
            rb.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}
