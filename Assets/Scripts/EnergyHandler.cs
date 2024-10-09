using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class EnergyHandler : MonoBehaviour
{

    public GameObject Light;
    public GameObject LightHolder;
    public GameObject EnergyBar;

    public Camera Camera;

    public float hitboxDecrease = 0.85f;
    public float Energy = 100;
    public float EnergyIncrease = 10;
    public float EnergyDecrease = 15;
    public float MeteorDamage = 20;
    
    private bool IsInside;
    private float fovValue;

    void Start()
    {
        ResizeAndPos();
        UpdateFOV();
    }

    void Update()
    {
        if(IsInside){
            if (Energy < 100f){
            Energy+=EnergyIncrease*Time.deltaTime;
            }
            else{
                Energy = 100.0f;
            }
        }
        else if(!IsInside){
            Energy -=EnergyDecrease*Time.deltaTime;
            if(Energy<=0){
                Energy = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        EnergyBar.GetComponent<Image>().fillAmount = Energy / 100;

        MeteorCollide();

        UpdateFOV();
    }
    
    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
             IsInside = true;
        }
       
        // Light.GetComponent<Light>().color = Color.green;
    }

    void OnTriggerExit (Collider other)
    {
        // Light.GetComponent<Light>().color = Color.red;
         if(other.gameObject.CompareTag("Player")) {
             IsInside = false;
        }
    }

    void ResizeAndPos()
    {
        float energyDiameter;
        float energyPos;
        
        float planetDiameter = LightHolder.transform.localScale.y;
        float lightPos = Light.transform.localPosition.y * planetDiameter;
        float lightAngle = Light.GetComponent<Light>().spotAngle / 2 * hitboxDecrease;

        float a = - Mathf.Cos(lightAngle * Mathf.Deg2Rad)/Mathf.Sin(lightAngle * Mathf.Deg2Rad);
        float b = lightPos;
        float r = planetDiameter / 2;

        float distance = Mathf.Abs(b) / Mathf.Sqrt(Mathf.Pow(a,2)+1);

        if (distance > r || lightAngle >= 90)
        {
            /* Jeg orkede ikke at skrive et script for hvis lyskeglens sider ikke r�r planeten. 
             * Jeg g�r ikke ud fra at lyset skal v�re s� stort. */
            energyDiameter = 0;
            energyPos = 0;
        } else
        {
            float y = (b + Mathf.Sqrt(Mathf.Pow(a,4) * Mathf.Pow(r,2) - Mathf.Pow(a, 2) * Mathf.Pow(b, 2) + Mathf.Pow(r, 2) * Mathf.Pow(a, 2))) / (Mathf.Pow(a, 2) + 1);
            energyPos = y / planetDiameter;
            float x = Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(y, 2)); 
            energyDiameter = x * 2 / planetDiameter;
        }

        this.transform.localScale = new UnityEngine.Vector3(energyDiameter, energyDiameter, energyDiameter);
        this.transform.localPosition = new UnityEngine.Vector3(0, energyPos, 0);
    }

    void UpdateFOV()
    {
        float fovValue = Energy / (20/3) + 45;

        Camera.fieldOfView = fovValue;
    }

    void MeteorCollide()
    {
        if (GameObject.Find("Player").GetComponent<PlayerCollider>().hit)
        {
            Energy -= MeteorDamage;
        }
    }
}
