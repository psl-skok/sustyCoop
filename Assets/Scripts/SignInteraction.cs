using UnityEngine;
using TMPro;

public class SignInteraction : MonoBehaviour
{
    public GameObject promptCanvas; // Assign the panel GameObject in the Inspector
    public GameObject infoCanvas;
    private bool isPlayerNearby = false;

    void Start()
    {
        // Ensure the text panel is hidden initially
        promptCanvas.SetActive(false);
        infoCanvas.SetActive(false);
    }

    void Update()
    {
        // Check if the player is nearby and presses the interact key (e.g., "E")
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInfoCanvas();
        }
    }

    private void TogglePromptCanvas()
    {
        // Toggle the visibility of the text panel
        promptCanvas.SetActive(true);
    }

    private void ToggleInfoCanvas()
    {
        // Toggle the visibility of the text panel
        promptCanvas.SetActive(false);
        infoCanvas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the sign's trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Optionally display a hint like "Press E to interact"
            TogglePromptCanvas();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset when the player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            infoCanvas.SetActive(false);
            promptCanvas.SetActive(false);

        }
    }
}
