using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using System.Security.Cryptography;

public class EnergyHandler : MonoBehaviour
{

    public GameObject Light;
    public GameObject LightHolder;
    public GameObject HaveEnergy;
    public GameObject NoEnergy;
    public GameObject Player;

    public Camera Camera;

    public int NumOfObjects = 12;

    public float placePlayer = 0.75f;
    public float hitboxDecrease = 0.85f;
    public float Energy = 100;
    public float EnergyIncrease = 10;
    public float EnergyDecrease = 15;
    public float MeteorDamage = 20;
    
    private bool IsInside;
    private float fovValue;

    private GameObject[] energyindikatorer;
    
    void Start()
    {

        energyindikatorer = new GameObject[NumOfObjects];
        EnergyObjektSpawner();

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
        }

        if (Energy <= 0)
        {
            Energy = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Opdatere alle energi indikatorerne på spilleren
        UpdateEnergyDisplay(Energy);

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
             * Jeg går ikke ud fra at lyset skal være så stort. */
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

    //***************************************************************************//
    //        Spawner Energi indikatorerne ligeligt fordelt på spilleren         //
    //***************************************************************************//
    public void EnergyObjektSpawner()
    {
        float VinkelTrin = 360f/NumOfObjects;

        Vector3 center = Player.transform.position;

        //  Beregner hver indikators position og spawner dem på spilleren, det bliver beregnet som positionen på en cirkel, da vores spiller er cirkulær  //
        for(int i = 0; i < NumOfObjects; i++){
            
            float vinkel = i * VinkelTrin + 90;

            float radianer = vinkel * Mathf.Deg2Rad;

            float x = Mathf.Cos(radianer) * placePlayer;
            float z = Mathf.Sin(radianer) * placePlayer;
            Vector3 positionOnPlayer = new Vector3(center.x + x, center.y + 0.03f, center.z + z);

            GameObject indicator = Instantiate(HaveEnergy, positionOnPlayer, Quaternion.identity);
            indicator.transform.parent = Player.transform;
            indicator.transform.LookAt(center);

            energyindikatorer[i] = indicator;
        }

    }

    //***************************************************************************************//
    // Holder styr på hvilke af indikatorerne der skal være "aktive" og hvilke der ikke skal //
    //***************************************************************************************//
    public void UpdateEnergyDisplay(float playerEnergy)
    {

    //  Beregner hvor mange "aktive" indikatorer der skal være //
        int activeSegments = Mathf.RoundToInt((Energy / 100f) * NumOfObjects);

    //  finder ud af om indikatoren skal være "aktiv" eller ej
        for (int i = 0; i < NumOfObjects; i++)
        {
            if (i < activeSegments)
            {
                if (energyindikatorer[i].name != HaveEnergy.name + "(Clone)")
                {
                    ReplaceSegment(i, HaveEnergy);
                }
            }
            else
            {

                if (energyindikatorer[i].name != NoEnergy.name + "(Clone)")
                {
                    ReplaceSegment(i, NoEnergy);
                }
            }
        }
    }

    //***************************************************************************//
    //          Opdatere og erstatter de forskellige energi indikatorer          //
    //***************************************************************************//

    void ReplaceSegment(int index, GameObject newPrefab)
    {
    //  position og rotation af den eksisterende indikator
        Vector3 existingPosition = energyindikatorer[index].transform.position;
        Quaternion existingRotation = energyindikatorer[index].transform.rotation;

    //  Fjerner det nuværende seegment og tilføjer det nye
        Destroy(energyindikatorer[index]);

        GameObject nyIndikator = Instantiate(newPrefab, existingPosition, existingRotation);

        nyIndikator.transform.parent = Player.transform;

        energyindikatorer[index] = nyIndikator;
    }
}
