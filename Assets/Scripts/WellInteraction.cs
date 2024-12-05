using UnityEngine;
using TMPro;

public class WellInteraction : MonoBehaviour
{
    public GameObject promptCanvasCleanWater; // Assign the panel GameObject in the Inspector
    public GameObject promptCanvasDirtyWater;
    public GameObject infoCanvas;
    public GameObject warningCanvas;
    public GameObject dirtyCover;
    private bool isPlayerNearby = false;
    private bool isWaterDirty = true;

    void Start()
    {
        // Ensure the text panel is hidden initially
        promptCanvasCleanWater.SetActive(false);
        promptCanvasDirtyWater.SetActive(false);
        infoCanvas.SetActive(false);
        warningCanvas.SetActive(false);
    }

    void Update()
    {
        // Check if the player is nearby and presses the interact key (e.g., "E")
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isWaterDirty == false)
        {
            promptCanvasCleanWater.SetActive(false);
            infoCanvas.SetActive(true);
        }
        else if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isWaterDirty == true)
        {
            promptCanvasDirtyWater.SetActive(false);
            warningCanvas.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the sign's trigger area
        if (other.CompareTag("Player") && isWaterDirty == true)
        {
            isPlayerNearby = true;
            promptCanvasDirtyWater.SetActive(true);
        }
        else if(other.CompareTag("Player") && isWaterDirty == false)
        {
            isPlayerNearby = true;
            promptCanvasCleanWater.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset when the player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            infoCanvas.SetActive(false);
            promptCanvasDirtyWater.SetActive(false);
            promptCanvasCleanWater.SetActive(false);
            warningCanvas.SetActive(false);
        }
    }

    // Method to clean the well when all waste is collected
    public void CleanWell()
    {
        isWaterDirty = false;
        dirtyCover.SetActive(false);
    }
}
