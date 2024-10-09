using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{

    public Light playerLight;

    void Update()
    {
        float energy = GameObject.Find("EnergyHandler").GetComponent<EnergyHandler>().Energy;
        playerLight.intensity = (energy / (10 / 3)) + 10;
    }
}
