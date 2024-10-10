using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollider : MonoBehaviour
{
    //float energy = GameObject.Find("EnergyHandler").GetComponent<EnergyHandler>().Energy; 
    public bool hit;


    private void Update()
    {
        hit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            hit = true;
            Destroy(other.gameObject);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else
        {
            hit = false;
        }
    }
}
