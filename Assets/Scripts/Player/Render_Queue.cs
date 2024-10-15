using UnityEngine;

public class SetRenderQueue : MonoBehaviour
{
    public Material material;
    public int renderQueue = 3000; // Set your desired render queue value

    void Start()
    {
        if (material != null)
        {
            material.renderQueue = renderQueue;
        }
    }
}