using UnityEngine;

public class Flicker : MonoBehaviour
{
    public float flickerFrequency = 10f; // Frequency in Hz
    public Material material1; // First color/material
    public Material material2; // Second color/material

    private Renderer objRenderer;
    private bool isMaterial1 = true;
    private float timer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        objRenderer.material = material1;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / flickerFrequency)
        {
            timer = 0;
            isMaterial1 = !isMaterial1;
            objRenderer.material = isMaterial1 ? material1 : material2;
        }
    }
}
