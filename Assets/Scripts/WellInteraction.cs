using UnityEngine;
using TMPro;

public class WellInteraction : MonoBehaviour
{
    public GameObject promptCanvasCleanWater; // Assign the panel GameObject in the Inspector
    public GameObject promptCanvasDirtyWater;
    public GameObject infoCanvas;
    public GameObject warningCanvas;
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
            ToggleInfoCanvas();
        }
        else if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && isWaterDirty == true)
        {
            warningCanvas.SetActive(true);
        }
    }

    private void ToggleInfoCanvas()
    {
        // Toggle the visibility of the text panel
        promptCanvasCleanWater.SetActive(false);
        promptCanvasDirtyWater.SetActive(false);
        infoCanvas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the sign's trigger area
        if (other.CompareTag("Player") && isWaterDirty == true)
        {
            isPlayerNearby = true;
            promptCanvasDirtyWater.SetActive(false);
        }
        else if(other.CompareTag("Player") && isWaterDirty == false)
        {
            isPlayerNearby = true;
            promptCanvasCleanWater.SetActive(false);
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

        }
    }
}
