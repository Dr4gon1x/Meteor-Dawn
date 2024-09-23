using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 randomPoint = Random.onUnitSphere * 50;
        //Debug.Log($"Random point: X={randomPoint.x}, Y={randomPoint.y}, Z={randomPoint.z}");
        
        


    }
}
