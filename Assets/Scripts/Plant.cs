using UnityEngine;

public class Plant : MonoBehaviour
{
    public bool isBloomed = false; // Tracks if the plant is bloomed
    public GameObject bloomedState; // The object representing the bloomed state (e.g., a fully-grown plant)
    public GameObject unBloomedState; // The object representing the unbloomed state (e.g., a seed or small plant)

    void Start()
    {
        // Ensure the plant starts in its unbloomed state
        if (unBloomedState != null)
            unBloomedState.SetActive(true);
        
        if (bloomedState != null)
            bloomedState.SetActive(false);
    }

    // Call this method to water the plant
    public void WaterPlant()
    {
        if (!isBloomed)
        {
            isBloomed = true;
            if (unBloomedState != null)
                unBloomedState.SetActive(false);
            if (bloomedState != null)
                bloomedState.SetActive(true);
        }
    }
}
