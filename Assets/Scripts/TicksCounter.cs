using UnityEngine;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class TicksCounter : MonoBehaviour
{
    public TMP_Text ticksText;
    private int ticksPerSecond = 0;
    private float time = 0;

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.25f)
        {
            ticksPerSecond = (int) (1 / Time.deltaTime);
            ticksText.text = "Ticks per second: " + ticksPerSecond.ToString();
            time = 0;
        }
    }
}
