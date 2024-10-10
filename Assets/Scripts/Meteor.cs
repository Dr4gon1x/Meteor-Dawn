using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            // GameObject sm = GameObject.FindGameObjectWithTag("SoundManager");
            // if(sm != null) {
            //     sm.GetComponent<SoundManager>().PlaySound(SoundManager.SoundEffects.Meteor);
            // }
            Destroy(other.gameObject);
        }

    }
}
