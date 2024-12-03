using UnityEngine;

public class Flower : MonoBehaviour
{
    /*public Material healthyMaterial;
    public Material wiltedMaterial;

    private bool isWilted = false;
    private Soil soil; // To reset soil if flower wilts

    void Start()
    {
        Invoke("Wilt", Random.Range(5f, 10f)); // Wilt after a random time
    }

    public void SetSoil(Soil soil)
    {
        this.soil = soil;
    }

    public void Water()
    {
        if (isWilted)
        {
            isWilted = false;
            GetComponent<Renderer>().material = healthyMaterial; // Switch back to healthy state
            CancelInvoke("Wilt"); // Reset wilt timer
            Invoke("Wilt", Random.Range(5f, 10f)); // Restart wilt timer
        }
    }

    private void Wilt()
    {
        isWilted = true;
        GetComponent<Renderer>().material = wiltedMaterial; // Switch to wilted state
        Invoke("DestroyFlower", 5f); // Destroy flower if not watered in time
    }

    private void DestroyFlower()
    {
        if (isWilted)
        {
            soil.ResetSoil(); // Allow replanting
            Destroy(gameObject); // Destroy flower
        }
    }

    void OnMouseDown()
    {
        Water(); // Player can click to water the flower
    }*/
}
